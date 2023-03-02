using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Alien : Enemy
{
    Character victimCharacter;

    public override void AI_TakeTurn(GameManager gm, BattleManager bm)
    {
        StartCoroutine(SimulateThinking(() =>
        {
            // Generate Random for
            int index = Random.Range(0, 2);
            GameObject victim;
            if (index == 0)
            {
                victim = bm.localPlayer;
                victimCharacter = CharacterRegister.GLOBAL_PLAYER;
            }
            else
            {
                victim = bm.localCompanion;
                victimCharacter = CharacterRegister.GLOBAL_COMPANION;
            }
            GameObject actionObject = ActionRegister.ENEMY_SHOOT_ENERGY_WAVE.actionModule;
            GameObject instance = Instantiate(actionObject, new Vector3(actionObject.transform.position.x, 3, actionObject.transform.position.z), actionObject.transform.rotation);
            ShootEnergyWave shootEnergyWave = instance.GetComponent<ShootEnergyWave>();
            shootEnergyWave.setAttackPosition(transform);
            shootEnergyWave.setVictim(victim, victimCharacter);
            shootEnergyWave.Shoot();
            //throw new System.NotImplementedException();

            return 1;
        }));
        
    }
}