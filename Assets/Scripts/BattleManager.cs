using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Turn {
    public int damage;

    public Turn(int damage){
        this.damage = damage;
    }
}

public class BattleManager : MonoBehaviour
{

    public GameObject          localPlayer, localCompanion;
    public List<GameObject>    localEnemies;

    public Transform playerSpot, companionSpot;
    public Transform[] enemySpots;

    private static BattleManager instance;

    GameManager gm;
    UIManager ui;
    EnemyTypes[] enemies;

    int currentTurn = 0;
    bool refreshEnemies = true;

    public GameObject currentAttacker, currentVictim;
    public int selectedEnemy;
    Stack<GameObject> enemiesToGo;

    public static BattleManager Get(){
        return BattleManager.instance;
    }

    public static void Set(BattleManager instance){
        if(BattleManager.instance != null){ Destroy(BattleManager.instance); }
        BattleManager.instance = instance;
    }

    public float Turns(){
        if(CharacterRegister.GLOBAL_COMPANION == null){
            return localEnemies.Count + 1;
        } else {
            return localEnemies.Count + 2;
        }
        
    }

    public void Begin(EnemyTypes[] enemies){
        List<GameObject> enemyList = new List<GameObject>();
        Character[] _enemies = new Character[enemies.Length];

        // Switch from EnemyTypes to Character
        foreach(EnemyTypes enemy in enemies){
            switch(enemy){
                case EnemyTypes.Fish:
                    enemyList.Add(CharacterRegister.FISH.battle);
                    break;
                case EnemyTypes.Alien:
                    enemyList.Add(CharacterRegister.ALIEN.battle);
                    break;
            }
        }

        localEnemies = new List<GameObject>();
        for(int i = 0; i < enemyList.Count; i++){
            GameObject enemy = Instantiate(
                                            enemyList[i],
                                            enemySpots[(enemySpots.Length-1)-i].position,
                                            enemySpots[(enemySpots.Length-1)-i].rotation
                                        );
            localEnemies.Add(enemy);
        }

        localPlayer =    Instantiate(
                                        CharacterRegister.GLOBAL_PLAYER.battle,
                                        playerSpot.transform.position,
                                        playerSpot.transform.rotation
                                    );

        
        if(CharacterRegister.GLOBAL_COMPANION != null){
            localCompanion = Instantiate(
                                        CharacterRegister.GLOBAL_COMPANION.battle,
                                        companionSpot.transform.position,
                                        companionSpot.transform.rotation
                                    );
        }
        gm.SceneChanged();

        SwitchTurn();
    }

    public void Awake(){ BattleManager.Set(this); }

    public void Start(){
        ui = UIManager.Get();
        gm = GameManager.Get();
        ui.refreshBM(this);

        // initialize spots
        playerSpot = GameObject.FindWithTag("Player_Spot").GetComponent<Transform>();
        companionSpot = GameObject.FindWithTag("Companion_Spot").GetComponent<Transform>();
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy_Spot");
        
        // convert enemy GameObject[] to Transform[]
        Transform[] _enemySpots = new Transform[enemyObjects.Length];
        int i = 0;
        foreach(GameObject spot in enemyObjects){
            _enemySpots[i] = spot.transform;
            i++;
        }

        enemySpots = _enemySpots;
    }

    // Call this whenever the player turn switches
    private void SwitchTurn()
    {
        if(localEnemies.Count > 0){
                if (currentTurn == 0) // Player
            {
                refreshEnemies = true;
                currentAttacker = localPlayer;
                ui.ToggleWheelType(true);
                ui.playerWheel.gameObject.SetActive(true);
                currentVictim = localEnemies[0];
            }
            else if (currentTurn == 1 && CharacterRegister.GLOBAL_COMPANION != null) // Companion
            {
                Debug.Log("calling companion");
                currentAttacker = localCompanion;
                ui.ToggleWheelType(false);
                ui.companionWheel.gameObject.SetActive(true);
                currentVictim = localEnemies[0];

                // Implement companion attack (consider making Player Jump more generic)
            } else {
                ui.playerWheel.gameObject.SetActive(false);
                ui.companionWheel.gameObject.SetActive(false);

                if(refreshEnemies){
                    refreshEnemies = false;
                    enemiesToGo = new Stack<GameObject>();
                    foreach(GameObject enemy in localEnemies){ enemiesToGo.Push(enemy); }
                }

                if(enemiesToGo.Count > 0){
                    Enemy enemy = enemiesToGo.Pop().GetComponent<Enemy>();
                    enemy.AI_TakeTurn(gm, this);
                } else {
                    currentTurn = 0;
                }
            }
        } else {
            gm.UnloadScene(this.gameObject.scene.name);
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
            localEnemies.TrimExcess();
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
