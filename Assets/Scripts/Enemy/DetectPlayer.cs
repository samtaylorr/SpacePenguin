using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectPlayer : MonoBehaviour
{
    public EnemyTypes[] enemies;
    public bool triggered = false;

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player" && !triggered){
            triggered = true;
            StartCoroutine(LoadScene("MatrixArena"));
        }
    }

    private IEnumerator LoadScene(string lvl)
    {
        // Start loading the scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Additive);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
        BattleManager.Get().Begin(enemies);
        GameManager.Get().PauseEntities();
    }
}