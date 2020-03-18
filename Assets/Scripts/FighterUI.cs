using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterUI : MonoBehaviour
{
    private GameObject hpBar;
    private float hpBarStartingWidth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        hpBar = transform.GetChild(1).gameObject;
        hpBarStartingWidth = hpBar.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void UpdateHp(int hp)
    {
        hpBar.GetComponent<RectTransform>().sizeDelta = new Vector2(hp / 100f * hpBarStartingWidth, hpBar.GetComponent<RectTransform>().sizeDelta.y);
        if (hp >= 67)
        {
            hpBar.GetComponent<Image>().color = Color.green;
        } 
        else if (hp >= 34)
        {
            hpBar.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            hpBar.GetComponent<Image>().color = Color.red;
        }
    } 
}
