using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    private bool metBefore = false;
    private bool isAngry = false;

    private bool isInConversation = false;
    private bool showingSecondaryScreen;
    private bool showPlayer;
    private bool isPlayerChoosing;
    private bool shouldShowText;
    private bool showingText;
    private string textToShow;

    public GameObject prompt;

    public void Awake(){
        dialogueUIElements = GameManager.Get().GetDialogueUIElements();
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
        player = GameManager.Get().GetPlayer().GetComponent<PlayerMovement>();
        if (!isInConversation)
        {
            DialogueSystem.ResetConversation();
            isInConversation = true;
            player.setEnabled(false);
            (showPlayer ? dialogueUIElements.PlayerContainer : dialogueUIElements.NpcContainer).SetActive(true);
        }
    }

    // Shoots a spherical raycast to detect if the player is in the right distance
    // to activate dialogue
    public CollisionHit PlayerDetection()
    {
        CollisionHit result = new CollisionHit();

        if (transform.position.z >= movementBounds.max) { transform.position = new Vector3(transform.position.x, transform.position.y, movementBounds.max); }
        else if (transform.position.z <= movementBounds.min) { transform.position = new Vector3(transform.position.x, transform.position.y, movementBounds.min); }

        int layerMask = 1 << 7;

        RaycastHit left, right;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);


        Debug.DrawRay(pos, transform.TransformDirection(-Vector3.right) * collisionDistance, Color.red);
        Debug.DrawRay(pos, transform.TransformDirection(Vector3.right) * collisionDistance, Color.green);

        if (Physics.Raycast(pos, transform.TransformDirection(-Vector3.right), out left, collisionDistance, layerMask))
        {
            result.left = true;
        }
        else { result.left = false; }

        if (Physics.Raycast(pos, transform.TransformDirection(Vector3.right), out right, collisionDistance, layerMask))
        {
            result.right = true;
        }
        else { result.right = false; }

        return result;
    }

    // Update is called once per frame
    void Update()
    {
        CollisionHit result = PlayerDetection();

        if ((result.left || result.right) && !isInConversation)
        {
            ShowPrompt();
        }
        else
        {
            HidePrompt();
        }

        if (Input.GetKeyDown(KeyCode.E) && (result.left || result.right) && !isInConversation)
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
                dialogueUIElements.LineController.Initialize(currentLines);
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
