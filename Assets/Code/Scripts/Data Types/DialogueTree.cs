using UnityEngine;
using System.Collections.Generic;

public class DialogueTree
{
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

    public enum DialogueType
    {
        WakeUp,
        WelcomeMission,
        Ingenuity,
        Information,
        CollectMission
    }

    DialogueNode rootNode;
    DialogueNode currentNode;

    public DialogueTree(DialogueType type)
    {
        switch (type)
        {
            case DialogueType.WakeUp:
                BuildExampleDialogueTree();
                break;
            case DialogueType.WelcomeMission:
                ConversationWithCuriosity();
                break;
            case DialogueType.Ingenuity:
                RepairAndLaunchDialogue();
                break;
            case DialogueType.Information:
                ReturnFromMissionDialogue();
                break;
            case DialogueType.CollectMission:
                CollectMissionDialogue();
                break;
            default:
                BuildExampleDialogueTree();
                break;
        }

        currentNode = rootNode;
    }

    void CollectMissionDialogue()
    {
        rootNode = new DialogueNode("Hi. I am Perseverance.");
        var node1 = new DialogueNode("Can I help you?");
        var node2 = new DialogueNode("There are lots of rovers on Mars.");
        var node3 = new DialogueNode("Perseverance, Ingenuity and Curiosity");
        var node4 = new DialogueNode("The most common compound on Mars is SiO₂.");
        var node5 = new DialogueNode("Second most common compound on Mars is Fe₂O₃.");
        var node6 = new DialogueNode("Third most common compound on Mars is Al₂O₃.");
        var node7 = new DialogueNode("You can use the rover to collect rocks.");

        var nodeFinal = new DialogueNode("You're welcome!");

        rootNode.Answers.Add("Hi Perseverance!", node1);
        rootNode.Answers.Add("Hello!", node1);

        node1.Answers.Add("What can you tell me about the rovers?", node2);
        node1.Answers.Add("How many rovers are there?", node2);

        node2.Answers.Add("Tell me some of their names.", node3);
        node2.Answers.Add("What are their names?", node3);

        node3.Answers.Add("What can you tell me about the compounds?", node4);
        node3.Answers.Add("What's common on Mars?", node4);

        node4.Answers.Add("What else can you tell me?", node5);
        node4.Answers.Add("What else is common?", node5);

        node5.Answers.Add("What is the third most common compound?", node6);
        node5.Answers.Add("And what else?", node6);

        node6.Answers.Add("How can I collect rocks?", node7);
        node6.Answers.Add("How do I collect rocks?", node7);

        node7.Answers.Add("Thank you!", nodeFinal);
        node7.Answers.Add("That's great!", nodeFinal);
    }

    void ConversationWithCuriosity()
    {
        rootNode = new DialogueNode("Hi. I am Curiosity.");
        var node1 = new DialogueNode("You are on Mars surface.");
        var node2 = new DialogueNode("Mars was named after the Roman god of war.");
        var node3 = new DialogueNode("First spacecraft to reach Mars was Viking 1.");

        var nodeFinal = new DialogueNode("You're welcome! Stay curious!");

        rootNode.Answers.Add("Where am I?", node1);
        rootNode.Answers.Add("Which planet is this?", node1);

        node1.Answers.Add("Is there any reason for that?", node2);
        node1.Answers.Add("Why was it named so?", node2);

        node2.Answers.Add("Which was the first spacecraft to reach Mars?", node3);
        node2.Answers.Add("What was the first spacecraft to reach Mars?", node3);

        node3.Answers.Add("Thank you!", nodeFinal);
        node3.Answers.Add("That's great!", nodeFinal);
    }

    void ReturnFromMissionDialogue()
    {
        rootNode = new DialogueNode("I'm back from the mission!");
        var node1 = new DialogueNode("Surface Temperature is -80 degrees.");
        var node2 = new DialogueNode("Proximity to Sun is 246.9 million km.");
        var node3 = new DialogueNode("Atmosphere Condition is 95% Carbon Dioxide.");
        var nodeFinal = new DialogueNode("It's a pleasure");

        rootNode.Answers.Add("What about Surface Temperature?", node1);
        rootNode.Answers.Add("Tell me about Surface Temperature", node1);

        node1.Answers.Add("What about Proximity to Sun?", node2);
        node1.Answers.Add("Tell me about Proximity to Sun", node2);

        node2.Answers.Add("What about Atmosphere Condition?", node3);
        node2.Answers.Add("Tell me about Atmosphere Condition", node3);

        node3.Answers.Add("Thank you.", nodeFinal);
        node3.Answers.Add("That's great.", nodeFinal);
    }

    void RepairAndLaunchDialogue()
    {
        rootNode = new DialogueNode("Thank you for repairing me!");
        var node1 = new DialogueNode("Who are you?");
        var node2 = new DialogueNode("I am Ingenuity. Can I help you?");
        var node3 = new DialogueNode("Can you tell me more about the mission?");
        var node4 = new DialogueNode("Anything else?");
        var node5 = new DialogueNode("I'm ready to launch!");
        var nodeFinal = new DialogueNode("Mission started!");

        rootNode.Answers.Add("You are welcome!", node1);
        rootNode.Answers.Add("It's my pleasure!", node1);

        node1.Answers.Add("I'm here to learn about Mars.", node2);
        node1.Answers.Add("I want to explore Mars.", node2);

        node2.Answers.Add("Yes please!", node3);
        node2.Answers.Add("It would be great!", node3);

        node3.Answers.Add("I need to learn about the atmosphere.", node4);
        node3.Answers.Add("Atmosphere is my main focus.", node4);

        node4.Answers.Add("Surface temperature and proximity to the sun.", node5);
        node4.Answers.Add("proximity to the sun and surface temperature.", node5);

        node5.Answers.Add("Great!", nodeFinal);
        node5.Answers.Add("Let's go!", nodeFinal);
    }

    void BuildExampleDialogueTree()
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

    public string GetQuestion()
    {
        return currentNode.Question;
    }

    public string GetAnswer1()
    {
        if (IsFinished())
        {
            return "Finish";
        }

        List<string> keys = new List<string>(currentNode.Answers.Keys);

        return keys[0];
    }

    public string GetAnswer2()
    {
        if (IsFinished())
        {
            return "Finish";
        }

        List<string> keys = new List<string>(currentNode.Answers.Keys);

        return keys[1];
    }

    public void SelectAnswer1()
    {
        if (currentNode.Answers.ContainsKey(GetAnswer1()))
        {
            currentNode = currentNode.Answers[GetAnswer1()];
        }
    }

    public void SelectAnswer2()
    {
        if (currentNode.Answers.ContainsKey(GetAnswer2()))
        {
            currentNode = currentNode.Answers[GetAnswer2()];
        }
    }

    public bool IsFinished()
    {
        return currentNode.Answers.Count == 0;
    }

    public void Reset()
    {
        currentNode = rootNode;
    }
}


