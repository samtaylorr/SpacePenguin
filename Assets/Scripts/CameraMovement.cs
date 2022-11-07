using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameManager gm;
    [SerializeField] Vector3 rightOffset;
    [SerializeField] Vector3 leftOffset;

    // Controls the speed of the camera following
    [SerializeField] float speed;
    // Controls the speed of switching between left/right
    [SerializeField] float lerpSpeed;
    float directionalOffset;
    bool isLeft;
    Vector3 offset, currentOffset;

    // Start is called before the first frame update
    void Awake()
    {
        gm = GameManager.Get();
    }

    public void UpdateDirection(bool isLeft){
        this.isLeft = isLeft;
    }

    void UpdateOffset(){
        if(isLeft){ offset = leftOffset; } else { offset = rightOffset; }
        currentOffset = new Vector3(Mathf.Lerp(currentOffset.x, offset.x, Time.deltaTime * lerpSpeed), offset.y, offset.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateOffset();
        transform.position = Vector3.Lerp(transform.position, new Vector3(
            gm.player.transform.position.x - currentOffset.x,
            transform.position.y,
            gm.player.transform.position.z - currentOffset.z), speed * Time.deltaTime);
    }

    void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, gm.player.transform.position.y - currentOffset.y, transform.position.z), speed * Time.deltaTime);
    }
}
