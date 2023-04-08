using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            GameManager.Get().PauseEntities();
            SceneManager.LoadScene("MatrixArena", LoadSceneMode.Additive);
        }
    }
}
