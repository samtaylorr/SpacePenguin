using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character {
    public GameObject overworld;
    public GameObject battle;
    public Sprite logo;
    float _HP, _maxHP;

    public Character(GameObject overworld, GameObject battle, Sprite logo, float HP, float maxHP){
        this.overworld = overworld;
        this.battle = battle;
        this.logo = logo;
        this._HP = HP;
        this._maxHP = maxHP;
    }

    public float HP(){ return _HP; }
    public float maxHP() { return _maxHP; }
    public void setHP(float HP){ this._HP = HP; }
    public void setMaxHP(float maxHP){ this._maxHP = maxHP; }
}

public static class ActionRegister {
    public static Action PLAYER_BASIC_JUMP = new Action(Resources.Load("Prefabs/Battle/Actions/PlayerJump") as GameObject, "Basic Jump");

    public static List<Action> PLAYER_JUMP = new List<Action> {
        PLAYER_BASIC_JUMP
    };

}

public static class CharacterRegister {
    public static Character GLOBAL_PLAYER = new Character(
                                                        Resources.Load("Prefabs/Overworld/Player") as GameObject,
                                                        Resources.Load("Prefabs/Battle/Player") as GameObject,
                                                        Resources.Load("Sprites/Dog.psd") as Sprite,
                                                        10, 10
                                                    );
    
    public static Character DOG_COMPANION = new Character(
                                                        Resources.Load("Prefabs/Overworld/Dog") as GameObject,
                                                        Resources.Load("Prefabs/Battle/Dog") as GameObject,
                                                        Resources.Load("Sprites/Penguin.psd") as Sprite,
                                                        5, 5
                                                     );

    public static Character GLOBAL_COMPANION = DOG_COMPANION;

    public static void UpdatePlayer     (Character player)            { GLOBAL_PLAYER        =    player;             }
    public static void UpdateCompanion  (Character companion)         { GLOBAL_COMPANION     =    companion;          }
}

public class GameManager : MonoBehaviour
{
    public bool moving, jumping;
    public GameObject player, companion;

    public void SceneChanged(){
        player = GameObject.FindGameObjectWithTag("Player");
        companion = GameObject.FindGameObjectWithTag("Companion");
    }

    public GameObject GetPlayer(){
        return player;
    }

    public GameObject GetCompanion(){
        return companion;
    }

    public void Awake(){
        SceneChanged();
    }

    static public GameManager Get(){
        return GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void UpdateDirections(bool isLeft){
        CameraMovement cam = Camera.main.GetComponent<CameraMovement>();
        if(cam != null){ cam.UpdateDirection(isLeft); }
        companion.GetComponent<CompanionMovement>().UpdateDirection(isLeft);
    }
}