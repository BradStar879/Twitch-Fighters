using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterUI : MonoBehaviour
{
    private GameObject hpBar;
    private GameObject specialBar;
    [SerializeField] GameObject[] winCircleBackgrounds;
    private Image[] winCircles; 
    private float hpBarStartingWidth;
    private float specialBarStartingWidth;

    public void Init()
    {
        GameManager gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
        int roundsToWin = gameManager.GetRoundsToWin();
        hpBar = transform.GetChild(2).gameObject;
        hpBarStartingWidth = hpBar.GetComponent<RectTransform>().sizeDelta.x;
        specialBar = transform.GetChild(4).gameObject;
        specialBarStartingWidth = specialBar.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 0; i < 3; i++)
        {
            if (i < roundsToWin)
            {
                winCircleBackgrounds[i].SetActive(true);
            }
            else
            {
                winCircleBackgrounds[i].SetActive(false);
            }
        }
        winCircles = new Image[3];
        winCircles[0] = transform.GetChild(8).gameObject.GetComponent<Image>();
        winCircles[1] = transform.GetChild(9).gameObject.GetComponent<Image>();
        winCircles[2] = transform.GetChild(10).gameObject.GetComponent<Image>();
        ResetWinCircles();
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

    public void UpdateSpecial(int special)
    {
        specialBar.GetComponent<RectTransform>().sizeDelta = new Vector2(special / 50f * specialBarStartingWidth, specialBar.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void UpdateWins(int wins)
    {
        winCircles[wins - 1].enabled = true;
    }

    public void ResetWinCircles()
    {
        foreach (Image winCircle in winCircles)
        {
            winCircle.enabled = false;
        }
    }
}
