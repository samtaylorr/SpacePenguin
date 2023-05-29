using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character {
    public GameObject battle;
    public Sprite logo;
    float _HP, _maxHP;

    public Character(GameObject battle, Sprite logo, float HP, float maxHP){
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

// THIS CLASS IS TEMPORARY.
// EVENTUALLY WE SHOULD GET THIS FROM EITHER PLAYERPREFS OR AN ENCRYPTED JSON FILE DEPENDENT ON SAVES.
// BUT FOR NOW WE CAN JUST PULL PLAYER AND COMPANION ACTIONS FROM THIS REGISTER

public static class ActionRegister {
    public static Action PLAYER_BASIC_JUMP = new Action(Resources.Load("Prefabs/Battle/Actions/PlayerJump") as GameObject, "Basic Jump");
    public static Action ENEMY_SHOOT_ENERGY_WAVE = new Action(Resources.Load("Prefabs/Battle/Actions/EnergyWave") as GameObject, "Shoot Energy Wave");

    public static List<Action> PLAYER_JUMP = new List<Action> {
        PLAYER_BASIC_JUMP
    };

}

// WE USE THIS TO PULL IN SPAWNABLE CHARACTERS WHEN WE LOAD A NEW SCENE

public static class CharacterRegister {
    public static Character GLOBAL_PLAYER = new Character(
                                                        Resources.Load("Prefabs/Battle/Player") as GameObject, // BATTLE CHARACTER TO SPAWN
                                                        Resources.Load("Sprites/Dog.psd") as Sprite, // SPRITE
                                                        10, 10
                                                    );
    
    public static Character DOG_COMPANION = new Character(
                                                        Resources.Load("Prefabs/Battle/Dog") as GameObject,
                                                        Resources.Load("Sprites/Penguin.psd") as Sprite,
                                                        5, 5
                                                     );

    public static Character FISH = new Character          (
                                                        Resources.Load("Prefabs/Battle/Fish") as GameObject,
                                                        Resources.Load("Sprites/Fish.psd") as Sprite,
                                                        2, 2
                                                    );

    public static Character ALIEN = new Character          (
                                                        Resources.Load("Prefabs/Battle/Alien") as GameObject,
                                                        Resources.Load("Sprites/Fish.psd") as Sprite,
                                                        2, 2
                                                    );

    public static Character GLOBAL_COMPANION = null;

    // TODO: USE THESE FUNCTIONS TO SWAP PLAYERS AND CHARACTERS IN A UI MENU IN THE OVERWORLD OR IN BATTLE AS A TURN

    public static void UpdatePlayer     (Character player)            { GLOBAL_PLAYER        =    player;             }
    public static void UpdateCompanion  (Character companion)         { GLOBAL_COMPANION     =    companion;          }
}


public enum Music
{
    MenuMusic,
    Snow,
    SnowPlucks,
    ATalkingDog,
    TechnoticEscapism,
    PenguinDojo,
    Abduction
}

[System.Serializable]
public enum EnemyTypes
{
    Alien,
    Fish
}

[RequireComponent(typeof(DialogueUIElements))]
public class GameManager : MonoBehaviour
{
    public bool moving, jumping;
    public GameObject player, companion, camera;
    public TMPro.TMP_Text prompt;
    AudioSource audioSource;
    public Music currentSong;
    BattleManager bm;
    [SerializeField] UIManager ui;
    private static GameManager gm;

    Stack<GameObject> enemyCache;

    public void SceneChanged(Vector3 playerPosition){
        player.transform.position = playerPosition;
        return;
    }

    public UIManager UIManager(){
        return ui;
    }

    public void SceneChanged(){
        player.transform.position = new Vector3(0,0,0);
        return;
    }

    public GameObject GetPlayer(){
        return player;
    }

    public void SwitchMusic(Music music)
    {
        if(currentSong != music){
            switch (music)
            {
                case Music.MenuMusic:
                    audioSource.clip = (Resources.Load("Music/00 - Space Penguin") as AudioClip);
                    break;
                case Music.Snow:
                    audioSource.clip = (Resources.Load("Music/Area 01 - Snow") as AudioClip);
                    break;
                case Music.SnowPlucks:
                    audioSource.clip = (Resources.Load("Music/Area 01b - Snow Plucks") as AudioClip);
                    break;
                case Music.PenguinDojo:
                    audioSource.clip = (Resources.Load("Music/Battle 02 - Penguin Dojo") as AudioClip);
                    break;
                case Music.ATalkingDog:
                    audioSource.clip = (Resources.Load("Music/Area 02 - A Talking Dog") as AudioClip);
                    break;
                case Music.TechnoticEscapism:
                    audioSource.clip = (Resources.Load("Music/Battle 01 - Technotic Escapism") as AudioClip);
                    break;
                case Music.Abduction:
                    audioSource.clip = (Resources.Load("Music/Area 01c - The Abduction Song") as AudioClip);
                    break;
            }
            audioSource.Play();
            currentSong = music;
        }
    }

    public void PauseEntities(GameObject enemyToDestroy){
        player.GetComponent<PlayerMovement>().setEnabled(false);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCache = new Stack<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            if(enemy != enemyToDestroy){
                enemyCache.Push(enemy);
                enemy.SetActive(false);
            }
        }
    }

    public IEnumerator LoadBattle(string lvl, EnemyTypes[] enemies, Vector3 playerState, GameObject enemyToDestroy)
    {
        // Start loading the scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Additive);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
        PauseEntities(enemyToDestroy);
        player.transform.position = playerState;
        Destroy(enemyToDestroy);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(lvl));
        BattleManager.Get().Begin(enemies);
    }

    public void ResumeEntities(){
        player.GetComponent<PlayerMovement>().setEnabled(true);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        while(enemyCache.Count > 0){
            enemyCache.Pop().SetActive(true);
        }
    }

    public DialogueUIElements GetDialogueUIElements(){
        return GetComponent<DialogueUIElements>();
    }

    public GameObject GetCompanion(){
        return companion;
    }

    public void Awake(){
        gm = this;
        audioSource = GetComponent<AudioSource>();
        SceneChanged();
    }

    static public GameManager Get(){
        return gm;
    }
    
    public void UpdateDirections(bool isLeft){
        CameraMovement cam = camera.GetComponent<CameraMovement>();
        cam.UpdateDirection(isLeft);
        if(companion != null){ companion.GetComponent<CompanionMovement>().UpdateDirection(isLeft); }
    }

    public void UnloadScene(string lvl)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(lvl));
        ResumeEntities();
        StartCoroutine(Enum_UnloadScene(lvl));
    }

    public IEnumerator Enum_UnloadScene(string lvl)
    {
        // Start loading the scene
        AsyncOperation asyncUnloadLevel = SceneManager.UnloadSceneAsync(lvl);
        // Wait until the level finish loading
        while (!asyncUnloadLevel.isDone)
            yield return null;
    }
}