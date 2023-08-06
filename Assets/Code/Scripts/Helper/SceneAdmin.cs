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
                spaceShipUI.SetActive(false);
                Transform viewer = GameObject.Find("Viewer").transform;
                spaceshipInsideOutsideController.GoOutside();
                spaceship.GetComponentsInChildren<Camera>()[0].enabled = false;
                GameObject ISS_Wrapper = GameObject.Find("ISS_Wrapper");
                Destroy(ISS_Wrapper.transform.GetChild(1).GetChild(1).gameObject);
                GameObject playerNew = Instantiate(playerAndCamera, spaceship.position + new Vector3(0, 0, 10), Quaternion.identity);
                playerNew.SetActive(true);
                //InstantiateStateDrivenCam(playerNew);
                //set child
                viewer.SetParent(playerNew.transform.GetChild(1));
                viewer.localPosition = new Vector3(0, 0, 0);

                GameObject manager = GameObject.Find("Manager");
                foreach (Transform child in manager.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
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
