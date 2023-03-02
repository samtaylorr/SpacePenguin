using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnergyWave : ActionModule
{
    GameManager gm;
    Transform attackPosition;
    SpriteRenderer sr;
    public int damage = 1;
    Character victimCharacter;

    bool shoot = false;
    public void setVictim(GameObject v, Character vC)
    {
        attacker = v;
        victimCharacter = vC;
        Debug.Log("Set target to: " + v.name);
    }
    bool hasArrived(Vector3 p1, Vector3 p2)
    {
        Debug.Log(Vector3.Distance(p1, p2));
        if (Vector3.Distance(p1, p2) < 3) { return true; }
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
        // sr.sortingLayerName = "Attack";

        shoot = true;
    }

    private void Update()
    {
        if (shoot)
        {
            if (hasArrived(this.transform.position, attacker.transform.position))
            {
                Debug.Log("hit");
                if (shoot) { shoot = false; }
                SpawnHit(damage);

                bm.EndTurn(new Turn(damage), victimCharacter);
                Destroy(this.gameObject);
            }
            transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(attacker.transform.position.x, 3, attacker.transform.position.z), Time.deltaTime * 50);
            
        }
    }
}