using UnityEngine;
using UnityEngine.VFX;

public class SendMission : Mission
{
    private DialogueTree dialogueTreeSender;
    private DialogueTree dialogueTreeInfo;
    private DialogueTree dialogueTree;
    public GameObject dialoguePanel;
    private VisualEffect visualEffect;
    [SerializeField]
    private int rocksNeeded = 3;

    enum SendMissionState
    {
        Broken,
        NotStarted,
        Flying,
        Started,
        Finished
    }

    SendMissionState state = SendMissionState.Broken;

    // UI elements
    private DialoguePanel dialoguePanelScript;

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
        visualEffect = GetComponentInChildren<VisualEffect>();
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

    public void Fix()
    {
        int rocksCollected = CollectedRocks();
        if (rocksCollected >= rocksNeeded)
        {
            state = SendMissionState.NotStarted;
            visualEffect.Stop();
            dialoguePanelScript.openDialogueText.text = "Press E to talk to Ingenuity";
        }
        else
        {
            dialoguePanelScript.openDialogueText.text = "Press E to fix ( " + rocksCollected + " / " + rocksNeeded + " )";
        }
    }

    public void StartMission()
    {
        state = SendMissionState.Started;
        dialogueTree = dialogueTreeInfo;
    }

    private int CollectedRocks()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f, 1 << LayerMask.NameToLayer("Collectable"));
        return hitColliders.Length;
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
                    ingenuity.Fly();
                    state = SendMissionState.Flying;
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
            if (state == SendMissionState.Broken)
            {
                dialoguePanelScript.openDialogueText.text = "Press E to fix ( " + CollectedRocks() + " / " + rocksNeeded + " )";
                dialoguePanelScript.openDialogueText.gameObject.SetActive(true);
            }
            else if (state == SendMissionState.Flying || state == SendMissionState.Finished)
            {
                dialoguePanelScript.openDialogueText.gameObject.SetActive(false);
            }
            else
            {
                dialoguePanelScript.openDialogueText.text = "Press E to talk to Ingenuity";
                dialoguePanelScript.openDialogueText.gameObject.SetActive(true);
            }

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
            if (state == SendMissionState.Broken)
            {
                Fix();
                return;
            }
            else if (state == SendMissionState.Flying)
            {
                return;
            }
            else if (state == SendMissionState.Finished)
            {
                return;
            }

            StartDialogue();
        }
    }
}
