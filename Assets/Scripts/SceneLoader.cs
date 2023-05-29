using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum Scenes
{
    Area1Cutscene,
    Area1,
    Area2,
    AbductionCutscene,
    PrisonCellSpaceship,
    DemoEnd
}

public class SceneLoader : MonoBehaviour
{
    private Vector3 position = new Vector3(0,0,0);

    public void SetLoadPosition(int x) {
        Vector3 player = GameManager.Get().GetPlayer().transform.position;
        position = new Vector3(x, player.y, player.z);
    }

    public void LoadScene(Scenes scene)
    {
        GameManager.Get().GetDialogueUIElements().CutsceneContainer.SetActive(false);
        switch (scene)
        {
            case Scenes.Area1Cutscene:
                GameManager.Get().SwitchMusic(Music.SnowPlucks);
                SceneManager.LoadScene("Scenes/Cutscenes/Introduction");
                GameManager.Get().SceneChanged(position);
                break;
            case Scenes.Area1:
                GameManager.Get().SwitchMusic(Music.Snow);
                SceneManager.LoadScene("Scenes/SnowArea01");
                GameManager.Get().SceneChanged(position);
                break;
            case Scenes.Area2:
                GameManager.Get().SwitchMusic(Music.Snow);
                SceneManager.LoadScene("Scenes/SnowArea02");
                GameManager.Get().SceneChanged(position);
                break;
            case Scenes.AbductionCutscene:
                GameManager.Get().SwitchMusic(Music.Abduction);
                SceneManager.LoadScene("Scenes/Cutscenes/BeamCutscene");
                GameManager.Get().SceneChanged(position);
                break;
            case Scenes.PrisonCellSpaceship:
                GameManager.Get().SwitchMusic(Music.ATalkingDog);
                SceneManager.LoadScene("Scenes/Cutscenes/PrisonCell");
                GameManager.Get().SceneChanged(position);
                break;
            case Scenes.DemoEnd:
                GameManager.Get().SwitchMusic(Music.PenguinDojo);
                SceneManager.LoadScene("Scenes/DemoEnd");
                GameManager.Get().SceneChanged(position);
                break;
        }
    }


    public void Area1Cutscene() => LoadScene(Scenes.Area1Cutscene);
    public void Area1() => LoadScene(Scenes.Area1);
    public void Area2() => LoadScene(Scenes.Area2);
    public void AbductionCutscene() => LoadScene(Scenes.AbductionCutscene);
    public void PrisonCellSpaceship() => LoadScene(Scenes.PrisonCellSpaceship);
    public void DemoEnd() => LoadScene(Scenes.DemoEnd);
}
