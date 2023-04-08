using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum Scenes
{
    [SerializeField] Area1Cutscene,
    [SerializeField] Area1,
    [SerializeField] Area2
}

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
                GameManager.Get().SceneChanged();
                break;
            case Scenes.Area1:
                GameManager.Get().SwitchMusic(Music.Snow);
                SceneManager.LoadScene("Scenes/SnowArea01");
                GameManager.Get().SceneChanged();
                break;
            case Scenes.Area2:
                GameManager.Get().SwitchMusic(Music.Snow);
                SceneManager.LoadScene("Scenes/SnowArea02");
                GameManager.Get().SceneChanged();
                break;
        }
    }


    public void Area1Cutscene() => LoadScene(Scenes.Area1Cutscene);
    public void Area1() => LoadScene(Scenes.Area1);
    public void Area2() => LoadScene(Scenes.Area2);
}
