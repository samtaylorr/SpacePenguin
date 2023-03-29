using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DialogueGraph.Runtime;

public class CutsceneDialogue : MonoBehaviour
{
    public RuntimeDialogueGraph DialogueSystem;

    DialogueUIElements dialogueUIElements;

    private bool isInConversation = false;
    private bool showPlayer;
    private bool isPlayerChoosing;
    private bool shouldShowText;
    private bool showingText;
    private string textToShow;
    public GameObject prompt;

    public void Awake()
    {
        dialogueUIElements = GameManager.Get().GetDialogueUIElements();
        Activate();
    }

    public void Activate()
    {
        if (!isInConversation)
        {
            DialogueSystem.ResetConversation();
            isInConversation = true;
            dialogueUIElements.CutsceneContainer.SetActive(true);
        }
    }

    // Shoots a spherical raycast to detect if the player is in the right distance
    // to activate dialogue

    // Update is called once per frame
    void Update()
    {
        if (!isInConversation || isPlayerChoosing) return;
        if (shouldShowText)
        {
            dialogueUIElements.CutsceneContainer.SetActive(true);
            dialogueUIElements.CutsceneText.text = textToShow;
            showingText = true;
            shouldShowText = false;
        }

        if (showingText)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                showingText = false;
                (showPlayer ? dialogueUIElements.PlayerContainer : dialogueUIElements.NpcContainer).SetActive(false);
                (showPlayer ? dialogueUIElements.PlayerText : dialogueUIElements.NpcText).gameObject.SetActive(false);
            }
        }
        else
        {
            if (DialogueSystem.IsConversationDone())
            {
                // Reset state
                isInConversation = false;
                showPlayer = false;
                isPlayerChoosing = false;
                shouldShowText = false;
                showingText = false;

                dialogueUIElements.PlayerContainer.SetActive(false);
                dialogueUIElements.NpcContainer.SetActive(false);
                dialogueUIElements.CutsceneContainer.SetActive(false);
                return;
            }

            var currentActor = DialogueSystem.GetCurrentActor();
            showPlayer = false;
            shouldShowText = true;
            textToShow = DialogueSystem.ProgressNpc();
            dialogueUIElements.CutsceneText.text = currentActor.Name;
        }

    }
}
