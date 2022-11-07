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

    public float turns;
    float currentTurn = 0;

    public GameObject currentAttacker, currentVictim;
    public int selectedEnemy;

    public static BattleManager Get(){
        return GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
    } 

    // Start is called before the first frame update
    void Awake()
    {
        gm = GameManager.Get();
        ui = UIManager.Get();

        localEnemies = new List<GameObject>();
        battle.player      =    CharacterRegister.GLOBAL_PLAYER.battle;
        battle.companion   =    CharacterRegister.GLOBAL_COMPANION.battle;

        localPlayer =    Instantiate(
                                        battle.player,
                                        spots.playerSpot.transform.position,
                                        spots.playerSpot.transform.rotation
                                    );

        localCompanion = Instantiate(
                                        battle.companion,
                                        spots.companionSpot.transform.position,
                                        spots.companionSpot.transform.rotation
                                    );
    
        for(int i = 0; i < battle.enemies.Length; i++){
            GameObject enemy = Instantiate(
                                            battle.enemies[i],
                                            spots.enemySpots[i].position,
                                            spots.enemySpots[i].rotation
                                        );
            localEnemies.Add(enemy);
        }

        turns = battle.enemies.Length + 2;

        gm.SceneChanged();

        SwitchTurn();
    }

    // Call this whenever the player turn switches
    private void SwitchTurn()
    {
        if (currentTurn == 0) // Player
        {
            currentAttacker = localPlayer;
            
            ui.playerWheel.gameObject.SetActive(true);
            currentVictim = localEnemies[selectedEnemy];
        }
        else if (currentTurn == 1) // Companion
        {
            currentAttacker = localCompanion;
            currentVictim = localEnemies[selectedEnemy];

            // Implement companion wheel - Task 1
            // Implement companion attack (consider making Player Jump more generic)
        }
    }

    

    public void EndPlayerTurn(Turn turn, Enemy enemy){
        enemy.hp -= turn.damage;
        if(enemy.hp <= 0){
            selectedEnemy = 0;
            localEnemies.Remove(enemy.gameObject);
            enemy.Kill();
        }

        if (currentTurn + 1 < turns)
        {
            currentTurn++;
        }
        else { currentTurn = 0; }

        SwitchTurn();
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

    // Update is called once per frame
    void Update(){
        
    }
}