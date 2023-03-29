using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum Scenes
{
    [SerializeField] Area1Cutscene,
    [SerializeField] Area1
}


/* HUGE TODO:
 * Remove player, camera and companion objects from SnowArea01 scene
 * Create a way to spawn player and companion objects to the new scene
 * I'm thinking including an additional flag to indicate to GameManager this needs to be done
 * Then, create a way to spawn the Main camera in and bind settings to the new characters.
 * This will be similarly done for loading in battles and switching back vice/versa
 * Once you've done this, you could theoretically make the entire game easily.
*/

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(Scenes scene)
    {
        GameManager.Get().GetDialogueUIElements().CutsceneContainer.SetActive(false);
        switch (scene)
        {
            case Scenes.Area1Cutscene:
                GameManager.Get().SwitchMusic(Music.SnowPlucks);
                SceneManager.LoadScene("Scenes/Cutscenes/Introduction");
                break;
            case Scenes.Area1:
                GameManager.Get().SwitchMusic(Music.Snow);
                SceneManager.LoadScene("Scenes/SnowArea01");
                break;
        }
    }


    public void Area1Cutscene() => LoadScene(Scenes.Area1Cutscene);
    public void Area1() => LoadScene(Scenes.Area1);
}
