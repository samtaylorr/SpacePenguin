using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WheelManager : MonoBehaviour
{
    public PlayerActions selectedAction = PlayerActions.PLAYER_JUMP;
    BattleManager bm;
    Animator anim;
    public bool active;

    public void SetBM(BattleManager bm){
        this.bm = bm;
    }

    public void ToggleActive(){
        active = !active;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(active){
            anim.enabled = true;
            if(Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")) { UIManager.Get().EnableActionMenu(selectedAction); active = false; }
        } else {
            anim.enabled = false;
        }
    }

    private void OnEnable() {
        active = true;
    }
}
