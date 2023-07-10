using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QA_Manager : MonoBehaviour
{
    public Text questionText;
    public Text answerText;
    public GameObject panel;
    public SpaceShipController shipController;
    public AsteroidCollision asteroidCollisionController;

    // Start is called before the first frame update
    private string [] questions = {
        "What is the capital of the United States?",
        "What is the capital of Canada?",
        "What is the capital of Mexico?",
        "What is the capital of Brazil?",
        "What is the capital of Argentina?",
        "What is the capital of Chile?",
        "What is the capital of Peru?",
        "What is the capital of Colombia?",
        "What is the capital of Venezuela?",
        "What is the capital of Ecuador?",
        "What is the capital of Bolivia?",
        "What is the capital of Paraguay?",
        "What is the capital of Uruguay?",
        "What is the capital of Guyana?",
        "What is the capital of Suriname?",
        "What is the capital of French Guiana?",
        "What is the capital of Cuba?",
        "What is the capital of Haiti?",
        "What is the capital of the Dominican Republic?",
        "What is the capital of Jamaica?",
        "What is the capital of Puerto Rico?",
        "What is the capital of the Bahamas?",
        "What is the capital of Barbados?",
        "What is the capital of Trinidad and Tobago?",
        "What is the capital of Antigua and Barbuda?",
        "What is the capital of Dominica?",
        "What is the capital of Saint Lucia?",
        "What is the capital of Saint Vincent and the Grenadines?",
        "What is the capital of Grenada?",
        "What is the capital of Saint Kitts and Nevis?",
        "What is the capital of Belize?",
        "What is the capital of El Salvador?",
        "What is the capital of Guatemala?",
        "What is the capital of Honduras?",
        "What is the capital of Nicaragua?",
        "What is the capital of Costa Rica?",
        "What is the capital of Panama?",
        "What is the capital of Argentina?",
        "What is the capital of Chile?",
        "What is the capital of Peru?",
        "What is the capital of Colombia?",
        "What is the capital of Venezuela?",
        "What is the capital of Ecuador?",
        "What is the capital of Bolivia?",
        "What is the capital of Paraguay?",
        "What is the capital of Uruguay?",
        "What is the capital of Guyana?",
        "What is the capital of Suriname?",
        "What is the capital of French Guiana?",
        "What is the capital of Cuba?",
        "What is the capital of Haiti?",
    };

    private string [] answers = {
        "Washington, D.C.",
        "Ottawa",
        "Mexico City",
        "Brasilia",
        "Buenos Aires",
        "Santiago",
        "Lima",
        "Bogota",
        "Caracas",
        "Quito",
        "La Paz",
        "Asuncion",
        "Montevideo",
        "Georgetown",
        "Paramaribo",
        "Cayenne",
        "Havana",
        "Port-au-Prince",
        "Santo Domingo",
        "Kingston",
        "San Juan",
        "Nassau",
        "Bridgetown",
        "Port of Spain",
        "Saint John's",
        "Roseau",
        "Castries",
        "Kingstown",
        "Saint George's",
        "Basseterre",
        "Belmopan",
        "San Salvador",
        "Guatemala City",
        "Tegucigalpa",
        "Managua",
        "San Jose",
        "Panama City",
        "Buenos Aires",
        "Santiago",
        "Lima",
        "Bogota",
        "Caracas",
        "Quito",
        "La Paz",
        "Asuncion",
        "Montevideo",
        "Georgetown",
        "Paramaribo",
        "Cayenne",
        "Havana",
        "Port-au-Prince",
    };

    private int showQuestionInterval = 10;
    private int startTime;
    private bool answeringQuestion = false;
    [SerializeField]
    private float asteroidSpawnDistance;
    private bool inputActive;

    void Start()
    {
        startTime = (int)Time.time;
    }

    private void OnEnable()
    {
        Invoke(nameof(EnableInput), 1);
    }

    private void OnDisable()
    {
        inputActive = false;
    }

    private void EnableInput()
    {
        inputActive = true;
    }


    // Update is called once per frame
    void Update()
    {


        if (Time.time - startTime > showQuestionInterval && !answeringQuestion)
        {
            CreateAsteroid();
            answeringQuestion = true;
            startTime = (int)Time.time;
            int questionIndex = Random.Range(0, questions.Length);
            questionText.text = questions[questionIndex];
        }
        
        if (answeringQuestion)
        {
            panel.gameObject.SetActive(true);
            shipController.DisableControls();
            InputHandler();
        }

        AsteroidCollision asteroidCollision = asteroidCollisionController.gameObject.GetComponent<AsteroidCollision>();
        if (asteroidCollision != null && asteroidCollision.isCrashed)
        {
            startTime = (int)Time.time;
            asteroidCollision.isCrashed = false;
            answeringQuestion = false;
            panel.gameObject.SetActive(false);
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02F;
            shipController.EnableControls();
        }
    }

    void CreateAsteroid()
    {
        GameObject asteroidGameObject = asteroidCollisionController.gameObject;
        GameObject shipGameObject = shipController.gameObject;
        AsteroidCollision asteroidCollision = asteroidGameObject.GetComponent<AsteroidCollision>();

        asteroidGameObject.transform.position = shipGameObject.transform.position + shipGameObject.transform.forward * asteroidSpawnDistance;

        asteroidGameObject.transform.localScale = Vector3.zero;
        StartCoroutine(asteroidCollision.MoveAsteroid(asteroidGameObject.transform.position, shipGameObject.transform, 2f));
        LeanTween.scale(asteroidGameObject, Vector3.one * 3, 2f).setEase(LeanTweenType.easeInOutCubic);
        
        asteroidGameObject.SetActive(true);


        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

    }

    void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (answerText.text == answers[1])
            {
                Debug.Log("Correct!");
                answeringQuestion = false;
                panel.gameObject.SetActive(false);
                shipController.EnableControls();
                asteroidCollisionController.gameObject.SetActive(false);
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02F;
            }
            else
            {
                Debug.Log("Incorrect!");

            }
            // Check if answer is correct
            // If correct, add to score
            // If incorrect, subtract from score
            // Display next question
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Display pause menu
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            answerText.text = answerText.text.Substring(0, answerText.text.Length - 1);
        }
        else if(inputActive) {

            answerText.text += Input.inputString;
        }
    }
}
