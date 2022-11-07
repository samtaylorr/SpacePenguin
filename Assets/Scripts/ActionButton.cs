using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionButton : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    GameObject actionObject;
    ActionMenu menu;

    public void Assign(string name, GameObject actionObject, ActionMenu menu){
        text.text = name;
        this.actionObject = actionObject;
        this.menu = menu;
    }

    public void StartAction(){
        Instantiate(actionObject, actionObject.transform.position, actionObject.transform.rotation);
        menu.gameObject.SetActive(false);
        UIManager.Get().playerWheel.gameObject.SetActive(false);
    }
}
