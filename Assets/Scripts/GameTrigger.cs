using UnityEngine;
using UnityEngine.Events;

public class GameTrigger : MonoBehaviour
{
    public UnityEvent onTrigger;

    void Awake(){
        gameObject.layer = 16;
    }

    void OnTriggerEnter(Collider collision){
        if(collision.gameObject.tag == "Player"){
            onTrigger.Invoke();
        }
    }
}
