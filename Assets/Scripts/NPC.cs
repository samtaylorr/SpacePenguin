using UnityEngine;
using DialogueGraph.Runtime;

public class NPC : MonoBehaviour
{
    public bool questMarker;
    public GameObject questMarkerPrefab;

    [Header("Detection")]
    [SerializeField] Bound movementBounds;
    [SerializeField] float collisionDistance = 3;

    public RuntimeDialogueGraph DialogueSystem;

    DialogueUIElements dialogueUIElements;
    PlayerMovement player;

    private bool inDistance;

    private bool metBefore = false;
    private bool isAngry = false;

    private bool isInConversation = false;
    private bool showingSecondaryScreen;
    private bool showPlayer;
    private bool isPlayerChoosing;
    private bool shouldShowText;
    private bool showingText;
    private string textToShow;

    [Header("Misceellanious")]
    public bool rayCastMode = true;

    public GameObject prompt;

    public void Start(){
        dialogueUIElements = GameManager.Get().GetDialogueUIElements();
        player = GameManager.Get().GetPlayer().GetComponent<PlayerMovement>();
    }
    public void ShowPrompt()
    {
        prompt.SetActive(true);
    }

    public void HidePrompt()
    {
        prompt.SetActive(false);
    }

    public void Activate()
    {
        if (!isInConversation)
        {
            DialogueSystem.ResetConversation();
            isInConversation = true;
            player.setEnabled(false);
            (showPlayer ? dialogueUIElements.PlayerContainer : dialogueUIElements.NpcContainer).SetActive(true);
        }
    }

    public void DeActivate()
    {
        DialogueSystem.EndConversation();
        isInConversation = false;
        dialogueUIElements.PlayerContainer.SetActive(false);
        dialogueUIElements.NpcContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(rayCastMode){
            if(Vector3.Distance(this.transform.position, player.transform.position) < 7){ inDistance = true; }
            else { inDistance = false; }

            if (inDistance && !isInConversation)
            {
                ShowPrompt();
            }
            else
            {
                HidePrompt();
            }

            if (Input.GetKeyDown(KeyCode.E) && inDistance && !isInConversation)
            {
                HidePrompt();
                Activate();
            }

            if (questMarker && !questMarkerPrefab.activeInHierarchy)
            {
                questMarkerPrefab.SetActive(true);
            }

            if (!questMarker && questMarkerPrefab.activeInHierarchy)
            {
                questMarkerPrefab.SetActive(false);
            }
        }

        if (!isInConversation || isPlayerChoosing) return;
        if (shouldShowText) {
            (showPlayer ? dialogueUIElements.PlayerContainer : dialogueUIElements.NpcContainer).SetActive(true);
            (showPlayer ? dialogueUIElements.PlayerText : dialogueUIElements.NpcText).gameObject.SetActive(true);
            (showPlayer ? dialogueUIElements.PlayerText : dialogueUIElements.NpcText).text = textToShow;
            showingText = true;
            shouldShowText = false;
        }

        if (showingText) {
            if (Input.GetKeyDown(KeyCode.E)) {
                showingText = false;
                (showPlayer ? dialogueUIElements.PlayerContainer : dialogueUIElements.NpcContainer).SetActive(false);
                (showPlayer ? dialogueUIElements.PlayerText : dialogueUIElements.NpcText).gameObject.SetActive(false);
            }
        } else {
            if (DialogueSystem.IsConversationDone()) {
                // Reset state
                isInConversation = false;
                player.setEnabled(true);
                showingSecondaryScreen = false;
                showPlayer = false;
                isPlayerChoosing = false;
                shouldShowText = false;
                showingText = false;

                dialogueUIElements.PlayerContainer.SetActive(false);
                dialogueUIElements.NpcContainer.SetActive(false);
                return;
            }

            var isNpc = DialogueSystem.IsCurrentNpc();
            if (isNpc) {
                var currentActor = DialogueSystem.GetCurrentActor();
                showPlayer = false;
                shouldShowText = true;
                textToShow = DialogueSystem.ProgressNpc();
                dialogueUIElements.NpcName.text = currentActor.Name;
            } else {
                var currentLines = DialogueSystem.GetCurrentLines();
                isPlayerChoosing = true;
                dialogueUIElements.PlayerContainer.SetActive(true);
                dialogueUIElements.LineController.gameObject.SetActive(true);
                dialogueUIElements.LineController.Initialize(currentLines, this);
            }
        }
        
    }

    public void PlayerSelect(int index) {
        dialogueUIElements.LineController.gameObject.SetActive(false);
        textToShow = DialogueSystem.ProgressSelf(index);
        isPlayerChoosing = false;
        shouldShowText = true;
        showPlayer = true;
    }

    public void SetMarker(bool marker){
        questMarker = marker;
    }

    public bool MetBefore(string node, int lineIndex) {
        return metBefore;
    }

    public bool Angry(string node, int lineIndex) {
        return isAngry;
    }

    public void Meet(string node, int lineIndex) {
        metBefore = true;
    }

    public void MakeAngry(string node, int lineIndex) {
        isAngry = true;
    }

    public void ClearAngry(string node, int lineIndex) {
        isAngry = false;
    }

    public void PlayGame(string node, int lineIndex) {
        showingSecondaryScreen = true;
        dialogueUIElements.SecondaryScreen.SetActive(true);

        dialogueUIElements.NpcContainer.SetActive(false);
        dialogueUIElements.PlayerContainer.gameObject.SetActive(false);
        showingText = false;
        dialogueUIElements.PlayerText.gameObject.SetActive(false);
        dialogueUIElements.NpcText.gameObject.SetActive(false);
    }

    public void OpenShop(string node, int lineIndex) {
        showingSecondaryScreen = true;
        dialogueUIElements.SecondaryScreen.SetActive(true);

        dialogueUIElements.NpcContainer.SetActive(false);
        dialogueUIElements.PlayerContainer.gameObject.SetActive(false);
        showingText = false;
        dialogueUIElements.PlayerText.gameObject.SetActive(false);
        dialogueUIElements.NpcText.gameObject.SetActive(false);
    }
}
