using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationManager : MonoBehaviour
{
    public Text answerText1;
    public Text answerText2;
    public Text questionText;

    // Define a dialogue node structure
    struct DialogueNode
    {
        public string Question;
        public Dictionary<string, DialogueNode> Answers;

        public DialogueNode(string question)
        {
            Question = question;
            Answers = new Dictionary<string, DialogueNode>();
        }
    }

    // Define the root of the dialogue tree
    DialogueNode rootNode;

    // Track the current node
    DialogueNode currentNode;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize dialogue tree
        InitDialogueTree();

        // Set current node to root node and display question and answers
        currentNode = rootNode;
        DisplayCurrentNode();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnClickAnswer1();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnClickAnswer2();
        }
    }

    void InitDialogueTree()
    {
        rootNode = new DialogueNode("You finally woke up.");

        var node1 = new DialogueNode("You are in ISS. Are you ready to get back on mission?");
        var node2 = new DialogueNode("I am assistant AI of this ship. Don't you remember?");

        var node1a = new DialogueNode("Great! Are you familiar with your tasks?");
        var node1b = new DialogueNode("Sure, take your time. Inform me when you are ready.");

        var node2a = new DialogueNode("Fantastic! Do you recall your objectives?");
        var node2b = new DialogueNode("Don't worry. Your memory will return in time. Are you fit for duty now?");

        var nodeFinal = new DialogueNode("Let's start!");

        node1.Answers.Add("Yes, let's start the mission.", node1a);
        node1.Answers.Add("I need some more rest.", node1b);

        node1a.Answers.Add("Yes, I remember.", nodeFinal);
        node1a.Answers.Add("No, I need a refresher.", nodeFinal);
        
        node1b.Answers.Add("I'm ready now.", nodeFinal);
        node1b.Answers.Add("I need a little more time.", nodeFinal);

        node2.Answers.Add("I remember now, let's start the mission.", node2a);
        node2.Answers.Add("I can't remember anything.", node2b);

        node2a.Answers.Add("Yes, I remember my objectives.", nodeFinal);
        node2a.Answers.Add("No, I need to review them.", nodeFinal);
        
        node2b.Answers.Add("Yes, I can continue.", nodeFinal);
        node2b.Answers.Add("No, I need more rest.", nodeFinal);

        rootNode.Answers.Add("Where am I?", node1);
        rootNode.Answers.Add("Who are you?", node2);
    }


    void DisplayCurrentNode()
    {
        questionText.text = currentNode.Question;

        // Check if there are any answers
        if(currentNode.Answers.Count > 0)
        {
            List<string> keys = new List<string>(currentNode.Answers.Keys);
            answerText1.text = keys[0];
            answerText2.text = keys[1];
        }
        else
        {
            // If there are no more answers, hide the answer texts
            answerText1.text = "";
            answerText2.text = "";
        }
    }

    public void OnClickAnswer1()
    {
        // Check if there is a next node for answer 1 before trying to access it
        if(currentNode.Answers.ContainsKey(answerText1.text))
        {
            // Go to next node based on answer 1 and update the display
            currentNode = currentNode.Answers[answerText1.text];
            DisplayCurrentNode();
        }
    }

    public void OnClickAnswer2()
    {
        // Check if there is a next node for answer 2 before trying to access it
        if(currentNode.Answers.ContainsKey(answerText2.text))
        {
            // Go to next node based on answer 2 and update the display
            currentNode = currentNode.Answers[answerText2.text];
            DisplayCurrentNode();
        }
    }
}
