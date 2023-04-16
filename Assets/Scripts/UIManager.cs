using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text PenguinHPBar;
    [SerializeField] TMP_Text CompanionHPBar;
    [SerializeField] GameObject CompanionBar;
    public Animator playerWheel, companionWheel, currentWheel;
    [SerializeField] ActionMenu actionMenu;
    [SerializeField] ArrowManager selectionArrow;

    static public UIManager Get(){
        return GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CharacterRegister.GLOBAL_COMPANION == null)  { CompanionBar.SetActive(false); }
        else                                            { CompanionHPBar.text = CharacterRegister.GLOBAL_COMPANION.HP() + "/" + CharacterRegister.GLOBAL_COMPANION.maxHP(); }
        PenguinHPBar.text = CharacterRegister.GLOBAL_PLAYER.HP() + "/" + CharacterRegister.GLOBAL_PLAYER.maxHP();

        bool isActive = currentWheel.gameObject.GetComponent<WheelManager>().active;

        bool horizontal    =    Input.GetButtonDown("Horizontal");
        bool vertical      =    Input.GetButtonDown("Vertical");

        if(horizontal && isActive){
            float axis        =       Input.GetAxis("Horizontal");
            if(axis>0)              { currentWheel.SetTrigger("Next");         }
            else                    { currentWheel.SetTrigger("Previous");     }
            currentWheel.SetTrigger("Switch");
        }

        if(vertical && isActive){
            float axis        =       Input.GetAxis("Vertical");
            if(axis>0)              { currentWheel.SetTrigger("Next");         }
            else                    { currentWheel.SetTrigger("Previous");     }
            currentWheel.SetTrigger("Switch");
        }

    }

    public void ToggleWheelActive(){
        currentWheel.GetComponent<WheelManager>().ToggleActive();
    }

    public void SetPlayerWheelActive(BattleManager bm, bool isActive){

    }

    public void ToggleWheelType(bool isPlayer){
        if(isPlayer){ currentWheel = playerWheel;    }
        else        { currentWheel = companionWheel; }
    }

    public void EnableActionMenu(PlayerActions type){
        ToggleWheelActive();
        actionMenu.Reset();
        if(type == PlayerActions.PLAYER_JUMP){
            foreach(Action a in ActionRegister.PLAYER_JUMP){
                actionMenu.Add(a);
            }
        }
        actionMenu.gameObject.SetActive(true);
    }

    public void refreshBM(BattleManager bm){
        selectionArrow.SetBM(bm);
    }
}
