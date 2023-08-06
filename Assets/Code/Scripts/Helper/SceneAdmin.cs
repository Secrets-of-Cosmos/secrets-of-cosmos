using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Cinemachine;

[System.Serializable]
public struct SceneObject
{
    public Transform planet;
    public string sceneName;
    public float threshold;
    public Material skybox;
    public int ambientIntensity;
}

public class SceneAdmin : MonoBehaviour
{
    public Transform spaceship; // Uzay aracının Transform component'ı
    public GameObject[] shouldDisable;
    public Animator transitionAnimator; // Fade efekti için bir Animator component'ı
    private bool isLoading = false; // Kilit mekanizması olarak kullanılacak değişken
    public SceneObject[] sceneObjects;
    public InsideOutsideController issInsideOutsideController;
    public InsideOutsideController spaceshipInsideOutsideController;
    public GameObject playerPrefab;
    public GameObject playerStateDrivenCamPrefab;
    public GameObject playerAndCamera;
    public GameObject spaceShipUI;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        var currentScene = SceneManager.GetActiveScene();
        //Debug.Log(currentScene.name);
        // Tüm gezegenleri kontrol et
        if (currentScene.name == "Solar System")
        {
            CheckPlanets();

            if (Input.GetKeyDown(KeyCode.O))
            {
                issInsideOutsideController.GoOutside();
                spaceship.gameObject.SetActive(true);
                spaceShipUI.SetActive(true);
            }
        }
        if (currentScene.name == "Solar System" && spaceshipInsideOutsideController.inside)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Transform viewer = GameObject.Find("Viewer").transform;
                spaceshipInsideOutsideController.GoOutside();
                spaceship.GetComponentsInChildren<Camera>()[0].enabled = false;
                GameObject playerNew = Instantiate(playerAndCamera, spaceship.position + new Vector3(0, 0, 10), Quaternion.identity);
                //InstantiateStateDrivenCam(playerNew);
                //set child
                viewer.SetParent(playerNew.transform.GetChild(1));
                viewer.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    void InstantiateStateDrivenCam(GameObject player)
    {
        PapermanAC controller = player.GetComponent<PapermanAC>();
        GameObject stateDrivenCam = Instantiate(playerStateDrivenCamPrefab, spaceship.position + new Vector3(0, 0, 10), Quaternion.identity);

        CinemachineFreeLook cmFreeLook = stateDrivenCam.transform.GetChild(0).gameObject.GetComponent<CinemachineFreeLook>();
        cmFreeLook.m_Follow = player.transform.GetChild(5); // Getting the FPCamFocus
        cmFreeLook.m_LookAt = player.transform.GetChild(5); // Getting the FPCamFocus

        controller.vcam = stateDrivenCam.transform.GetChild(1).gameObject.GetComponent<CinemachineVirtualCamera>();
        controller.vcam.m_Follow = player.transform.GetChild(5); // Getting the FPCamFocus
        controller.vcam.m_LookAt = player.transform.GetChild(5); // Getting the FPCamFocus
    }

    void CheckPlanets()
    {
        for (int i = 0; i < sceneObjects.Length; i++)
        {
            float distance = Vector3.Distance(spaceship.position, sceneObjects[i].planet.position);

            // Eğer uzay aracı bir gezegene yeterince yaklaştıysa, sahneyi yükle
            if (distance < sceneObjects[i].threshold)
            {
                StartCoroutine(LoadPlanetScene(i));
                break;
            }
        }
    }

    public void LoadSolarSystemScene()
    {
        SceneManager.LoadScene("Solar System");

        spaceship = GameObject.Find("Spaceship").transform;
    }

    IEnumerator LoadPlanetScene(int planetIndex)
    {
        if (isLoading)
        {
            yield break;
        }

        isLoading = true;
        // Fade-out animasyonunu başlat
        transitionAnimator.SetTrigger("Start");

        // Animasyon bitene kadar bekle
        yield return new WaitForSeconds(1f);


        for (int i = 0; i < shouldDisable.Length; i++)
            shouldDisable[i].SetActive(false);

        // Gezegen indeksine bağlı olarak uygun sahneyi yükle
        string sceneName = sceneObjects[planetIndex].sceneName;
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Fade-in animasyonunu başlat
        transitionAnimator.SetTrigger("End");
        RenderSettings.ambientIntensity = sceneObjects[planetIndex].ambientIntensity;
        RenderSettings.skybox = sceneObjects[planetIndex].skybox;
        spaceship.transform.position = new Vector3(0, 1000, 0);
        spaceship.transform.rotation = Quaternion.Euler(90, 0, 0);
        spaceship.GetComponent<Rigidbody>().useGravity = true;

        Transform viewer = GameObject.Find("Viewer").transform;
        //set child 
        viewer.SetParent(spaceship);
        viewer.localPosition = new Vector3(0, 0, 0);
    }
}
