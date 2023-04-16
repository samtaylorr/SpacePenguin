using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeveloperMode : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.F)){
            GameManager.Get().SwitchMusic(Music.ATalkingDog);
            SceneManager.LoadScene("Scenes/Cutscenes/PrisonCell");
            GameManager.Get().SceneChanged();
        }
    }
}
