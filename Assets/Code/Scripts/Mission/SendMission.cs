using UnityEngine;
public class SendMission : Mission
{
    private DialogueTree dialogueTreeSender;
    private DialogueTree dialogueTreeInfo;
    private DialogueTree dialogueTree;
    public GameObject dialoguePanel;

    enum SendMissionState
    {
        NotStarted,
        Started,
        Finished
    }

    SendMissionState state = SendMissionState.NotStarted;

    // UI elements
    public DialoguePanel dialoguePanelScript;

    // Distance to player to start dialogue
    public float nearDistance = 5.0f;

    private string dialogueStartedBy = "";
    private Rigidbody rb;
    private Ingenuity ingenuity;
    // Start is called before the first frame update
    void Start()
    {
        InitializeTalkMission();
        AddMission();
        dialogueTreeSender = new DialogueTree(DialogueTree.DialogueType.Ingenuity);
        dialogueTreeInfo = new DialogueTree(DialogueTree.DialogueType.Information);
        dialogueTree = dialogueTreeSender;
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
        missionType = MissionType.Send;
        ingenuity = GetComponent<Ingenuity>();
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
                if (state == SendMissionState.NotStarted)
                {
                    state = SendMissionState.Started;
                    dialogueTree = dialogueTreeInfo;
                    ingenuity.Fly();
                    return;
                }
                else if (state == SendMissionState.Started)
                {
                    state = SendMissionState.Finished;
                    CompleteMission();
                }
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

    private void FreezeRigidbody(bool isFrozen)
    {
        if (rb != null)
        {
            rb.constraints = isFrozen ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
        }
    }
}
