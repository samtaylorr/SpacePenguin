using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnergyWave : ActionModule
{
    GameManager gm;
    Transform attackPosition;
    GameObject target;
    SpriteRenderer sr;

    bool shoot = false;
    public void setTarget(GameObject v)
    {
        target = v;
        Debug.Log("Set target to: " + v.name);
    }
    bool hasArrived(Vector3 p1, Vector3 p2)
    {
        if (Vector3.Distance(p1, p2) < 1) { return true; }
        else { return false; }
    }

    public void setAttackPosition(Transform t)
    {
        attackPosition = t;
    }

    public void Shoot()
    {
        transform.position = attackPosition.position;
        transform.rotation = attackPosition.rotation;
        sr.sortingLayerName = "Attack";

        shoot = true;
    }

    private void Update()
    {
        if (shoot)
        {
            if (hasArrived(this.transform.position, target.transform.position))
            {
                Debug.Log("hit");
                if (shoot) { shoot = false; }
            }
            transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(target.transform.position.x, 3, target.transform.position.z), Time.deltaTime * 50);
            
        }
    }

    IEnumerator FadeOut()
    {
        for(int i = 255; i > 0; i=i-5)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, i);
            yield return new WaitForSeconds(0.1f);
        }
    }
}