using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool questMarker;
    public GameObject questMarkerPrefab;
    NPCDialogue dialogue;

    [Header("Detection")]
    [SerializeField] Bound movementBounds;
    [SerializeField] float collisionDistance = 3;

    // Shoots a spherical raycast to detect if the player is in the right distance
    // to activate dialogue
    public CollisionHit PlayerDetection()
    {
        CollisionHit result = new CollisionHit();

        if (transform.position.z >= movementBounds.max) { transform.position = new Vector3(transform.position.x, transform.position.y, movementBounds.max); }
        else if (transform.position.z <= movementBounds.min) { transform.position = new Vector3(transform.position.x, transform.position.y, movementBounds.min); }

        int layerMask = 1 << 7;

        RaycastHit left, right;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);


        Debug.DrawRay(pos, transform.TransformDirection(-Vector3.right) * collisionDistance, Color.red);
        Debug.DrawRay(pos, transform.TransformDirection(Vector3.right) * collisionDistance, Color.green);

        if (Physics.Raycast(pos, transform.TransformDirection(-Vector3.right), out left, collisionDistance, layerMask))
        {
            result.left = true;
        }
        else { result.left = false; }

        if (Physics.Raycast(pos, transform.TransformDirection(Vector3.right), out right, collisionDistance, layerMask))
        {
            result.right = true;
        }
        else { result.right = false; }

        return result;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogue == null) { dialogue = GameManager.Get().dialogue; }
        else if (dialogue != null && !dialogue.isInConversation)
        {
            
            CollisionHit result = PlayerDetection();

            if ((result.left || result.right) && dialogue != null)
            {
                dialogue.ShowPrompt();
            }
            else if ((!result.left && !result.right) && dialogue != null)
            {
                dialogue.HidePrompt();
            }

            if (Input.GetKeyDown(KeyCode.E) && (result.left || result.right))
            {
                dialogue.HidePrompt();
                dialogue.Activate();
            }

            if (questMarker && !questMarkerPrefab.activeInHierarchy)
            {
                questMarkerPrefab.SetActive(true);
            }

            if (!questMarker && questMarkerPrefab.activeInHierarchy)
            {
                questMarkerPrefab.SetActive(false);
            }
        }
        
    }
}
