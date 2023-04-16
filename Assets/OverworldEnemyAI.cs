using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OverworldEnemyAI : MonoBehaviour
{
    [SerializeField] GameObject fish;
    [SerializeField] Animator anim;
    [SerializeField] Transform[] points;
    private int currentPoint;
    [SerializeField] float speed;

    private void Walk(){
        anim.SetBool("isWalking", true);
        if(currentPoint == 0){ anim.SetBool("Right", false); }
        else { anim.SetBool("Right", true); }
        if(Vector3.Distance(fish.transform.position, points[currentPoint].position) <= 7){
            if(currentPoint+1 >= points.Length){
                currentPoint = 0;
            } else {
                currentPoint++;
            }
        }
        fish.transform.position = Vector3.MoveTowards(fish.transform.position, new Vector3(points[currentPoint].position.x, fish.transform.position.y, points[currentPoint].position.z), speed * Time.deltaTime);
    }

    private void LateUpdate() {
        Walk();
    }

    private void Awake(){
        // Start at random places to avoid walking together at the same time.
        currentPoint = Random.Range(0,2);
        fish.transform.position = new Vector3((points[0].position.x+points[1].position.x)/Random.Range(0.0f, 8.0f), fish.transform.position.y, points[currentPoint].position.z);
    }

    
}
