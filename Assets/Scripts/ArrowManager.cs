using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    BattleManager bm;

    public void SetBM(BattleManager bm){ this.bm = bm; }

    // Update is called once per frame
    void Update()
    {
        UpdateArrow();
    }

    public void UpdateArrow(){
        transform.position = new Vector3(bm.currentVictim.transform.position.x,
                                        bm.currentVictim.transform.position.y + 8,
                                        bm.currentVictim.transform.position.z);
    }
}
