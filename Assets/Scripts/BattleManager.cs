using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Battle {
    public GameObject       player, companion;
    public GameObject[]     enemies;
}

[System.Serializable]
public struct Spots {
    public Transform        playerSpot, companionSpot;
    public Transform[]      enemySpots;
}

[System.Serializable]
public struct Turn {
    public int damage;

    public Turn(int damage){
        this.damage = damage;
    }
}

public class BattleManager : MonoBehaviour
{
    [SerializeField]    Spots           spots;
    [SerializeField]    Battle          battle;

    public GameObject          localPlayer, localCompanion;
    public List<GameObject>    localEnemies;

    GameManager gm;
    UIManager ui;

    int currentTurn = 0;

    public GameObject currentAttacker, currentVictim;
    public int selectedEnemy;

    public static BattleManager Get(){
        return GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
    }

    public float Turns(){
        if(battle.companion == null){
            return localEnemies.Count + 1;
        } else {
            return localEnemies.Count + 2;
        }
        
    }

    // Start is called before the first frame update
    void Awake()
    {
        gm = GameManager.Get();
        ui = UIManager.Get();

        localEnemies = new List<GameObject>();
        battle.player      =    CharacterRegister.GLOBAL_PLAYER.battle;
        if(battle.companion != null) { battle.companion   =    CharacterRegister.GLOBAL_COMPANION.battle; }

        localPlayer =    Instantiate(
                                        battle.player,
                                        spots.playerSpot.transform.position,
                                        spots.playerSpot.transform.rotation
                                    );

        
        if(battle.companion != null){
            localCompanion = Instantiate(
                                        battle.companion,
                                        spots.companionSpot.transform.position,
                                        spots.companionSpot.transform.rotation
                                    );
        }
        

        for(int i = 0; i < battle.enemies.Length; i++){
            GameObject enemy = Instantiate(
                                            battle.enemies[i],
                                            spots.enemySpots[i].position,
                                            spots.enemySpots[i].rotation
                                        );
            localEnemies.Add(enemy);
        }

        gm.SceneChanged();

        SwitchTurn();
    }

    // Call this whenever the player turn switches
    private void SwitchTurn()
    {
        if(localEnemies.Count > 0){
                if (currentTurn == 0) // Player
            {
                currentAttacker = localPlayer;
                ui.ToggleWheelType(true);
                ui.playerWheel.gameObject.SetActive(true);
                currentVictim = localEnemies[0];
            }
            else if (currentTurn == 1 && battle.companion != null) // Companion
            {
                currentAttacker = localCompanion;
                ui.ToggleWheelType(false);
                ui.companionWheel.gameObject.SetActive(true);
                currentVictim = localEnemies[0];

                // Implement companion attack (consider making Player Jump more generic)
            } else {
                ui.playerWheel.gameObject.SetActive(false);
                ui.companionWheel.gameObject.SetActive(false);
                int index = currentTurn - 2;
                if (index > -1 && index < localEnemies.Count){
                    currentAttacker = localEnemies[index];
                    currentAttacker.GetComponent<Enemy>().AI_TakeTurn(gm, this);
                }
            }
        } else {
            Debug.Log("Battle over");
        }
    }

    void EndTurn()
    {
        if (currentTurn + 1 < Turns())
        {
            currentTurn++;
        }
        else { currentTurn = 0; }

        SwitchTurn();
    }


    public void EndTurn(Turn turn, Enemy enemy){
        enemy.hp -= turn.damage;
        if(enemy.hp <= 0){
            localEnemies.Remove(enemy.gameObject);
            enemy.Kill();
        }

        EndTurn();
    }

    public void EndTurn(Turn turn, Character player)
    {
        player.setHP(player.HP() - turn.damage);
        if (player.HP() <= 0)
        {
            Debug.Log("Handle game over");
        }

        EndTurn();
    }


    public void ChangeVictim(bool moveLeft){
        if(!moveLeft){
            if(selectedEnemy + 1 > localEnemies.Count-1){
            selectedEnemy = 0;
            } else {
                selectedEnemy++;
            }
        } else {
            if(selectedEnemy - 1 < 0){
                selectedEnemy = localEnemies.Count-1;
            } else {
                selectedEnemy--;
            }
        }

        currentVictim = localEnemies[selectedEnemy];
    }
}
