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

    private int showQuestionInterval = 5;
    private int startTime;
    private bool answeringQuestion = false;

    void Start()
    {
        startTime = (int)Time.time;
    }



    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > showQuestionInterval && !answeringQuestion)
        {
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
    }

    void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (answerText.text == answers[0])
            {
                Debug.Log("Correct!");
                answeringQuestion = false;
                panel.gameObject.SetActive(false);
                shipController.EnableControls();
            }
            else
            {
                Debug.Log("Incorrect!");
                shipController.ShakeShip();
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
        else {
            answerText.text += Input.inputString;
        }
    }
}
