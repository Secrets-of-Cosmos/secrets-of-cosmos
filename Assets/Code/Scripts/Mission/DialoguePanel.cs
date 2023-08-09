using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    public Text dialogueText;
    public Text answer1Text;
    public Text answer2Text;
    public TextMeshProUGUI openDialogueText;

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void SetAnswer1Text(string text)
    {
        answer1Text.text = text;
    }

    public void SetAnswer2Text(string text)
    {
        answer2Text.text = text;
    }

    public void SetOpenDialogueText(string text)
    {
        openDialogueText.text = text;
    }
}
