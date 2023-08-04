using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TalkMission : Mission
{
    private DialogueTree dialogueTree;
    public GameObject dialoguePanel;

    // UI elements
    public DialoguePanel dialoguePanelScript;

    // Distance to player to start dialogue
    public float nearDistance = 5.0f;

    private string dialogueStartedBy = "";
    // Start is called before the first frame update
    void Start()
    {
        InitializeTalkMission();
        AddMission();
        dialogueTree = new DialogueTree();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTalkMission();
        HandlePlayerNearby();
        HandlePlayerInteract();
        if (dialogueStartedBy == name)
        {
            HandleAnswerSelection();
        }
    }

    void InitializeTalkMission()
    {
        missionManager = GameObject.FindObjectOfType<MissionManager>();
        dialoguePanelScript = dialoguePanel.GetComponent<DialoguePanel>();
        missionType = MissionType.Talk;
        dialoguePanel.SetActive(false);
    }

    void UpdateTalkMission()
    {
        // Code to update a talk mission, for example, checking if the player has talked to a character
    }

    public void StartDialogue()
    {
        dialogueTree.Reset();
        dialoguePanel.SetActive(true);
        dialoguePanelScript.openDialogueText.gameObject.SetActive(false);
        dialoguePanelScript.SetDialogueText(dialogueTree.GetQuestion());
        dialoguePanelScript.SetAnswer1Text(dialogueTree.GetAnswer1());
        dialoguePanelScript.SetAnswer2Text(dialogueTree.GetAnswer2());
    }

    private void HandleAnswerSelection()
    {
        bool isAlpha1Pressed = Input.GetKeyDown(KeyCode.Alpha1);
        bool isAlpha2Pressed = Input.GetKeyDown(KeyCode.Alpha2);
        if (isAlpha1Pressed || isAlpha2Pressed)
        {
            if (dialogueTree.IsFinished())
            {
                dialoguePanel.SetActive(false);
                CompleteMission();
                return;
            }

            if (isAlpha1Pressed)
            {
                dialogueTree.SelectAnswer1();
            }
            else
            {
                dialogueTree.SelectAnswer2();
            }

            dialoguePanelScript.SetDialogueText(dialogueTree.GetQuestion());
            dialoguePanelScript.SetAnswer1Text(dialogueTree.GetAnswer1());
            dialoguePanelScript.SetAnswer2Text(dialogueTree.GetAnswer2());
        }
    }


    private void HandlePlayerNearby()
    {

        if (IsPlayerNearby() && !dialoguePanel.activeSelf && dialogueStartedBy == "")
        {
            dialogueStartedBy = name;
            dialoguePanelScript.openDialogueText.gameObject.SetActive(true);
        }
        else if (!IsPlayerNearby() && dialogueStartedBy == name)
        {
            dialogueStartedBy = "";
            dialoguePanelScript.openDialogueText.gameObject.SetActive(false);
        }
    }

    private void HandlePlayerInteract()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerNearby() && dialogueStartedBy == name)
        {
            StartDialogue();
        }
    }

    private bool IsPlayerNearby()
    {
        return Vector3.Distance(player.transform.position, transform.position) < nearDistance;
    }
}
