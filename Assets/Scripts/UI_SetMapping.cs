using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetMapping : MonoBehaviour
{
    public void SetMapping(ButtonManager.Mapping mapping){
        string path = ButtonManager.ButtonPath(mapping);
        Sprite btn = Resources.Load<Sprite>(path);
        GetComponent<Image>().sprite = btn;
    }
}
