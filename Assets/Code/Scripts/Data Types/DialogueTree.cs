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
        Information
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
                BuildExampleDialogueTree();
                break;
            case DialogueType.Ingenuity:
                BuildIngenuityDialogueTree();
                break;
            case DialogueType.Information:
                BuildInformationDialogueTree();
                break;
            default:
                BuildExampleDialogueTree();
                break;
        }

        currentNode = rootNode;
    }
    void BuildInformationDialogueTree()
    {
        rootNode = new DialogueNode("I analyzed the atmosphere. The data seems interesting.");

        var node1 = new DialogueNode("Would you like to see the detailed analysis?");
        var node2 = new DialogueNode("Do you need me to perform any specific actions based on the analysis?");

        var node1a = new DialogueNode("Here are the details: [insert detailed analysis here].");
        var node1b = new DialogueNode("Understood. Just let me know if you need more information later.");

        var node2a = new DialogueNode("Action completed. What would you like to do next?");
        var node2b = new DialogueNode("Okay, I'll stand by for further instructions.");

        var nodeFinal = new DialogueNode("Mission accomplished. Awaiting next command.");

        rootNode.Answers.Add("Can you show me the details?", node1);
        rootNode.Answers.Add("Is there any action I should take?", node2);

        node1.Answers.Add("Sure, here are the details.", node1a);
        node1.Answers.Add("No need for details right now.", node1b);

        node2.Answers.Add("Yes, please perform this action: [insert action here].", node2a);
        node2.Answers.Add("No, no action needed at this time.", node2b);

        node1a.Answers.Add("Thank you for the details.", nodeFinal);
        node1b.Answers.Add("Thank you, I'll reach out if I need anything.", nodeFinal);

        node2a.Answers.Add("Great job, thank you.", nodeFinal);
        node2b.Answers.Add("Thank you, I'll let you know if anything is needed.", nodeFinal);
    }


    void BuildIngenuityDialogueTree()
    {
        rootNode = new DialogueNode("Hi, I am Ingenuity helicopter. Who are you?");

        var node1 = new DialogueNode("Great! I will make my best to help you.");
        var node2 = new DialogueNode("Fantastic! I will make my best to help you.");

        var nodeFinal = new DialogueNode("Yep, let's fly!");

        rootNode.Answers.Add("I'm the operator assigned to control you.", node1);
        rootNode.Answers.Add("I'm a scientist working on this project.", node2);

        node1.Answers.Add("Are you ready to analysis?", nodeFinal);
        node1.Answers.Add("Shall we start the analysis?", nodeFinal);

        node2.Answers.Add("Shall we start the analysis?", nodeFinal);
        node2.Answers.Add("Are you ready to analysis?", nodeFinal);
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


