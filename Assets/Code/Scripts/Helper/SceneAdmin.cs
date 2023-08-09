using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

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
    public Transform outsideDoor;
    public TextMeshProUGUI enterExitText;
    public GameObject[] shouldDisable;
    public Animator transitionAnimator; // Fade efekti için bir Animator component'ı
    private bool isLoading = false; // Kilit mekanizması olarak kullanılacak değişken
    public SceneObject[] sceneObjects;
    public InsideOutsideController issInsideOutsideController;
    public InsideOutsideController spaceshipInsideOutsideController;
    public GameObject playerAndCamera;
    public GameObject spaceShipUI;
    public PapermanAC papermanAC;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (issInsideOutsideController.IsOutside() && spaceshipInsideOutsideController.IsInside())
        {
            CheckPlanets();
        }
        if (Input.GetKeyDown(KeyCode.P) && spaceshipInsideOutsideController.IsInside())
        {
            ExitSpaceShip();
        }
        else if (Input.GetKeyDown(KeyCode.F) && spaceshipInsideOutsideController.IsOutside())
        {
            if (issInsideOutsideController.IsInside())
            {
                issInsideOutsideController.GoOutside();
                spaceship.gameObject.SetActive(true);
                spaceShipUI.SetActive(true);
                enterExitText.text = "";
            }
            else if (CloseToSpaceship())
            {
                EnterSpaceship();
                enterExitText.text = "";
            }
        }

        if (issInsideOutsideController.IsInside())
        {
            CheckExitDoorText();
            ExitDoor();
        }
        else if (spaceshipInsideOutsideController.IsOutside())
        {
            if (CloseToSpaceship())
            {
                enterExitText.text = "Press F to go inside";
            }
            else
            {
                enterExitText.text = "";
            }
        }

    }

    private void ExitDoor()
    {
        if (CloseToExitDoor())
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                EnterSpaceship();
            }
        }
    }

    private void CheckExitDoorText()
    {
        if (CloseToExitDoor())
        {
            enterExitText.text = "Press F to go outside";
        }
        else
        {
            enterExitText.text = "";
        }
    }

    private void EnterSpaceship()
    {
        spaceShipUI.SetActive(true);
        spaceshipInsideOutsideController.GoInside();
        spaceship.GetComponent<Rigidbody>().velocity = Vector3.zero;
        spaceship.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        spaceship.GetComponentsInChildren<Camera>()[0].enabled = true;
        GameObject ISS_Wrapper = GameObject.Find("ISS_Wrapper");
        GameObject playerNew = GameObject.Find("Player");
        Destroy(playerNew);
        Transform viewer = GameObject.Find("Viewer").transform;
        viewer.SetParent(spaceship);
        viewer.localPosition = new Vector3(0, 0, 0);
        GameObject manager = GameObject.Find("Manager");
        foreach (Transform child in manager.transform)
        {
            child.gameObject.SetActive(false);
        }

    }

    private void ExitSpaceShip()
    {
        spaceShipUI.SetActive(false);
        spaceship.GetComponent<Rigidbody>().isKinematic = false;
        Transform viewer = GameObject.Find("Viewer").transform;
        spaceshipInsideOutsideController.GoOutside();
        spaceship.GetComponentsInChildren<Camera>()[0].enabled = false;
        GameObject ISS_Wrapper = GameObject.Find("ISS_Wrapper");
        if (ISS_Wrapper.transform.GetChild(1).childCount > 1)
        {
            Destroy(ISS_Wrapper.transform.GetChild(1).GetChild(1).gameObject);
        }
        else
        {
            Destroy(GameObject.Find("Player"));
        }
        GameObject playerNew = Instantiate(playerAndCamera, spaceship.position + new Vector3(0, 0, 10), Quaternion.identity);
        papermanAC = playerNew.GetComponentInChildren<PapermanAC>();
        playerNew.name = "Player";
        playerNew.SetActive(true);
        viewer.SetParent(playerNew.transform.GetChild(1));
        viewer.localPosition = new Vector3(0, 0, 0);

        GameObject manager = GameObject.Find("Manager");

        foreach (Transform child in manager.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private bool CloseToExitDoor()
    {
        return Vector3.Distance(papermanAC.transform.position, outsideDoor.position) < 5;
    }

    private bool CloseToSpaceship()
    {
        return Vector3.Distance(papermanAC.transform.position, spaceshipInsideOutsideController.GetOutsidePosition()) < 10;
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
