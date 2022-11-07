using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Hit : MonoBehaviour
{
    SpriteRenderer sprite;
    [SerializeField] TMP_Text hitFont;
    float alpha = 255;
    int dmg = 1;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Damage(int dmg){
        this.dmg = dmg;
        hitFont.text = dmg.ToString();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.Translate(transform.up * 15 * Time.deltaTime);
        Color tmp = new Color32(255,255,255,(byte)alpha);
        hitFont.color = tmp;
        sprite.color = tmp;
        alpha-=3;
        if(alpha <= 0){ Destroy(this.gameObject); }
    }
}
