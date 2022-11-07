using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenu : MonoBehaviour
{

    public GameObject actionButton, selectionArrow;
    public Transform content;
    List<GameObject> actions = new List<GameObject>();
    WheelManager currentWheel;
    BattleManager bm;

    float startY = 65, Y, gap = 40;

    void Awake(){
        Y = startY;
        if(bm == null){ bm = BattleManager.Get(); }
    }

    // Update is called once per frame
    void Update()
    {
        bool horizontal    =    Input.GetButtonDown("Horizontal");
        bool vertical      =    Input.GetButtonDown("Vertical");

        if(horizontal){
            float axis        =       Input.GetAxis("Horizontal");
            if(axis>0)                { bm.ChangeVictim(false);        }
            else                      { bm.ChangeVictim(true);       }
        }

        if(Input.GetButtonDown("Cancel")){
            Cancel();
        }
    }

    public void Cancel(){
        currentWheel.ToggleActive();
        this.gameObject.SetActive(false);
    }

    public void Reset(WheelManager wheel){
        currentWheel = wheel;
        foreach(GameObject a in actions){ Destroy(a);  }
        actions = new List<GameObject>();
        Y = startY;
    }

    public void Add(Action a){
        GameObject obj = Instantiate(actionButton, Vector3.zero, transform.rotation);
        actions.Add(obj);
        obj.GetComponent<ActionButton>().Assign(a.name, a.actionModule, this);
        obj.transform.SetParent(content, false);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,Y,1);
        Y -= gap;
    }

    private void OnEnable() {
        bm.selectedEnemy = bm.localEnemies.Count-1;
        selectionArrow.SetActive(true);
    }

    private void OnDisable() {
        selectionArrow.SetActive(false);
    }
}
