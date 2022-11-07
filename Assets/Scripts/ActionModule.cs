using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModule : MonoBehaviour
{
    public GameObject attacker;
    public GameObject victim;
    UI_Hit hit;
    public BattleManager bm;

    private void OnEnable() {
        bm = BattleManager.Get();
        UIManager.Get().playerWheel.gameObject.SetActive(false);
        attacker = bm.currentAttacker;
        victim = bm.currentVictim;
    }

    public void SpawnHit(int dmg){
        GameObject hit = Instantiate(Resources.Load("Prefabs/Battle/HitMarker"), attacker.transform.position, attacker.transform.rotation) as GameObject;
        hit.GetComponent<UI_Hit>().Damage(dmg);
    }
}