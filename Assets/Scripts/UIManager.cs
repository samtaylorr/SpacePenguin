using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text PenguinHPBar;
    [SerializeField] TMP_Text CompanionHPBar;
    public Animator playerWheel, companionWheel;
    [SerializeField] ActionMenu actionMenu;

    static public UIManager Get(){
        return GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        PenguinHPBar.text = CharacterRegister.GLOBAL_PLAYER.HP() + "/" + CharacterRegister.GLOBAL_PLAYER.maxHP();
        CompanionHPBar.text = CharacterRegister.GLOBAL_COMPANION.HP() + "/" + CharacterRegister.GLOBAL_COMPANION.maxHP();
        
        bool isActive = playerWheel.gameObject.GetComponent<WheelManager>().active;

        bool horizontal    =    Input.GetButtonDown("Horizontal");
        bool vertical      =    Input.GetButtonDown("Vertical");

        if(horizontal && isActive){
            float axis        =       Input.GetAxis("Horizontal");
            if(axis>0)              { playerWheel.SetTrigger("Next");     }
            else                    { playerWheel.SetTrigger("Previous");         }
            playerWheel.SetTrigger("Switch");
        }

        if(vertical && isActive){
            float axis        =       Input.GetAxis("Vertical");
            if(axis>0)              { playerWheel.SetTrigger("Next");     }
            else                    { playerWheel.SetTrigger("Previous");         }
            playerWheel.SetTrigger("Switch");
        }
        
    }

    public void EnableActionMenu(PlayerActions type){
        actionMenu.Reset(playerWheel.GetComponent<WheelManager>());
        if(type == PlayerActions.PLAYER_JUMP){
            foreach(Action a in ActionRegister.PLAYER_JUMP){
                actionMenu.Add(a);
            }
        }
        actionMenu.gameObject.SetActive(true);
    }
}