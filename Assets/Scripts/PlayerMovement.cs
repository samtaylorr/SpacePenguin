using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(EdgeDetection))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    public PlayerAnimationHandler animHandler;
    Rigidbody rb;
    EdgeDetection collisions;
    bool isGrounded;
    bool isLeft = false;
    CameraMovement mainCam;
    GameManager gm;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collisions = GetComponent<EdgeDetection>();
        gm = GameManager.Get();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Get input axis
        float horizontal    =   Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical      =   Input.GetAxis("Vertical")   * speed * Time.deltaTime;

        if(Input.GetButtonDown("Jump") && isGrounded){
            rb.AddForce(transform.up * jumpForce);
            animHandler.SetJump();
            isGrounded = false;
        }
        
        CollisionHit result = collisions.DetectCollision();
        if(horizontal <= 0 && result.left){ horizontal = 0; }
        if(horizontal >= 0 && result.right){ horizontal = 0; }

        transform.Translate(horizontal, 0, vertical);

        if(horizontal < 0 || vertical < 0){
            gm.moving = true;
            animHandler.SetMove();
        } else if(horizontal > 0 || vertical > 0){
            gm.moving = true;
            animHandler.SetMove();
        } else {
            gm.moving = false;
            animHandler.SetIdle();
        }

        if(horizontal > 0){ animHandler.SetRight(); isLeft = false; }
        else if(horizontal < 0){ animHandler.SetLeft(); isLeft = true; }
        gm.UpdateDirections(isLeft);

        if(vertical < 0){ animHandler.SetFront(); }
        else if(vertical > 0.05f){ animHandler.SetBack(); }
    }

    void FixedUpdate(){
        if(isGrounded){
            gm.jumping = false;
        } else {
            gm.jumping = true;
            rb.AddForce(-transform.up * jumpForce/16);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer >= 10){
            isGrounded = true;
            animHandler.SetGrounded();
        }
    }
}
