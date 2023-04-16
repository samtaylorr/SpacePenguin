using UnityEngine;
public class DetectPlayer : MonoBehaviour
{
    public EnemyTypes[] enemies;
    public bool triggered = false;

    Vector3 playerState;
    GameObject player;

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player" && !triggered){
            triggered = true;
            player = other.gameObject;
            playerState = player.transform.position;
            
            StartCoroutine(GameManager.Get().LoadBattle("Scenes/MatrixArena", enemies, playerState, this.transform.parent.gameObject));
        }
    }
}