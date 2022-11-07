using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp, dp, ap;
    Animator anim;

    void Awake (){
        anim = GetComponent<Animator>();
    }

    public void PlayHit(){
        anim.SetTrigger("Hit");
    }

    public void Kill(){
        anim.SetTrigger("Die");
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine(){
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
