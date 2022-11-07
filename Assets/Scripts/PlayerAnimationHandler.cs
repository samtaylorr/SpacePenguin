using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem dust;
    
    private void OnEnable() {
        anim = GetComponent<Animator>();
    }

    public void SetJump(){
        if (anim != null && anim.isActiveAndEnabled){
            anim.SetBool("Jumping", true);
        }
    }

    public void SetGrounded(){
        anim.SetBool("Jumping", false);
    }
    
    public void SetIdle(){
        if (anim != null && anim.isActiveAndEnabled){
            anim.SetBool("Moving", false);
        }
    }

    public void SetMove(){
        anim.SetBool("Moving", true);
    }

    public void SetLeft(){
        anim.SetBool("isLeft", true);
    }

    public void SetRight(){
        if (anim != null && anim.isActiveAndEnabled){
            anim.SetBool("isLeft", false);
        }
    }

    public void SetFront(){
        anim.SetBool("isFront", true);
    }

    public void SetBack(){
        anim.SetBool("isFront", false);
    }
}
