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

    DialogueNode rootNode;
    DialogueNode currentNode;

    public DialogueTree()
    {
        BuildExampleDialogueTree();
        currentNode = rootNode;
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


