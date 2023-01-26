using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bezier))]
public class Action_PlayerJump : ActionModule
{

    Vector3[] positions;
    bool arrived = false, pressed = false, combo = false, followPath = true;
    int i = 0;
    [SerializeField] PlayerAnimationHandler animHandler;
    public int damage = 2;
    [SerializeField] UI_SetMapping UIprompt;

   // Start is called before the first frame update
    void Start()
    {
        UIprompt.SetMapping(ButtonManager.ButtonMappings.Y_KEY);
        Vector3 victimPos = new Vector3(victim.transform.position.x + (victim.GetComponent<BoxCollider>().size.x/3),
                                         victim.transform.position.y + victim.GetComponent<BoxCollider>().size.y,
                                                                        victim.transform.position.z);

        positions = GetComponent<Bezier>().CalculateCurvePoints(100, attacker.transform.position, victimPos, 25);
        StartCoroutine(Jump());
        animHandler = attacker.GetComponent<MovementAbstract>().animHandler;
        animHandler.SetJump();
    }

    IEnumerator Timing(Transform img){
        if(!pressed){
            pressed=true;
            Vector3 before = img.localScale, after = new Vector3(img.localScale.x-0.2f,img.localScale.y-0.2f,img.localScale.z-0.2f);
            img.localScale = after;
            yield return new WaitForSeconds(0.2f);
            img.localScale = before;
        } else {
            yield return null;
        }

    }

    bool isComboReady(){
        if(Vector3.Distance(attacker.transform.position, positions[positions.Length-1]) < 1){ return true; }
        else { return false; }
    }

    bool hasArrived(Vector3 p1, Vector3 p2){
        if(Vector3.Distance(p1, p2) < 1){ return true; }
        else { return false; }
    }

    IEnumerator Jump(){
        while(i < positions.Length){
            yield return new WaitUntil(() => arrived);
            i++;
        }

        i--;
        SpawnHit(damage);
        victim.GetComponent<Enemy>().PlayHit();
        yield return new WaitForSeconds(0.05f);

        if(combo){
            Vector3 secondJump = new Vector3(attacker.transform.position.x, attacker.transform.position.y + 10f, attacker.transform.position.z);

            followPath = false;

            while(!hasArrived(attacker.transform.position, secondJump)){
                attacker.transform.position = Vector3.MoveTowards(attacker.transform.position, secondJump, Time.deltaTime * 25);
                yield return new WaitForEndOfFrame();
            }

            while(!hasArrived(attacker.transform.position, positions[i])){
                attacker.transform.position =Vector3.MoveTowards(attacker.transform.position, positions[i], Time.deltaTime * 20);
                yield return new WaitForEndOfFrame();
            }

            SpawnHit(damage);
            victim.GetComponent<Enemy>().PlayHit();
            yield return new WaitForSeconds(0.05f);
            damage*=damage;

            followPath =  true;
         }

        i--;

        while (i > 0){
            yield return new WaitUntil(() => arrived);
            i--;
        }
        animHandler.SetGrounded();
        bm.EndPlayerTurn(new Turn(damage), victim.GetComponent<Enemy>());
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(followPath){
            attacker.transform.position = Vector3.MoveTowards(attacker.transform.position, positions[i], Time.deltaTime * 50);
            arrived = hasArrived(attacker.transform.position, positions[i]);
        }

        if(isComboReady()){
            StartCoroutine(Timing(UIprompt.transform));
        }

        if(isComboReady() && Input.GetButtonDown("Jump")){ combo = true; }
    }
}
