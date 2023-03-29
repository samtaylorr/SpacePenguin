using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUIElements : MonoBehaviour
{
    public LineController LineController;

    [Header("UI References")]
    public GameObject SecondaryScreen;
    public GameObject PlayerContainer;
    public GameObject NpcContainer;
    public TMP_Text PlayerText;
    public TMP_Text NpcText;
    public TMP_Text NpcName;
    public TMP_Text Prompt;

    [Header("Cutscene References")]
    public GameObject CutsceneContainer;
    public TMP_Text CutsceneText;
}
