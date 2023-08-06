using UnityEngine;
public class TalkMission : Mission
{
    private DialogueTree dialogueTree;
    public GameObject dialoguePanel;

    // UI elements
    private DialoguePanel dialoguePanelScript;
    private string dialogueStartedBy = "";
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        InitializeTalkMission();
        AddMission();
        dialogueTree = new DialogueTree(DialogueTree.DialogueType.WelcomeMission);
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerNearby();
        HandlePlayerInteract();
        if (dialogueStartedBy == name)
        {
            HandleAnswerSelection();
        }
    }

    void InitializeTalkMission()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
        }
        missionManager = GameObject.FindObjectOfType<MissionManager>();
        dialoguePanelScript = dialoguePanel.GetComponent<DialoguePanel>();
        missionType = MissionType.Talk;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue()
    {
        dialogueTree.Reset();
        dialoguePanel.SetActive(true);
        dialoguePanelScript.openDialogueText.gameObject.SetActive(false);
        dialoguePanelScript.SetDialogueText(dialogueTree.GetQuestion());
        dialoguePanelScript.SetAnswer1Text(dialogueTree.GetAnswer1());
        dialoguePanelScript.SetAnswer2Text(dialogueTree.GetAnswer2());
        FreezeRigidbody(true);
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
                FreezeRigidbody(false);
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
            dialoguePanelScript.openDialogueText.text = "Press E to talk";
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
}
