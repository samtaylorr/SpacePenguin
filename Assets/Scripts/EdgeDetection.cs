using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Bound {
    [SerializeField] public float min, max, trigger;
}

[System.Serializable]
public struct CollisionHit {
    public bool left, right, front, back;
}

public class EdgeDetection : MonoBehaviour
{
    [SerializeField] Bound movementBounds;
    [SerializeField] float collisionDistance = 3;

    public CollisionHit DetectCollision(){
        CollisionHit result = new CollisionHit();

        if(transform.position.z >= movementBounds.max){ transform.position = new Vector3(transform.position.x, transform.position.y, movementBounds.max); }
        else if(transform.position.z <= movementBounds.min){ transform.position = new Vector3(transform.position.x, transform.position.y, movementBounds.min); }

        int layerMask = (1 << 7);
        layerMask |= (1 << 16);
        layerMask = ~layerMask;

        RaycastHit left, right;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);

        if(Physics.Raycast(pos, transform.TransformDirection(-Vector3.right), out left, collisionDistance, layerMask)){
            result.left = true;
        } else { result.left = false; }

        if(Physics.Raycast(pos, transform.TransformDirection(Vector3.right), out right, collisionDistance, layerMask)){
            result.right = true;
        } else { result.right = false; }

        return result;
    }
}
