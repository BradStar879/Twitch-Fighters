using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboMenu : BaseMenuScript
{
    [SerializeField] Button basicCombosButton;
    [SerializeField] Button advancedCombosButton;
    [SerializeField] Button backButton;

    [SerializeField] GameObject basicCombos;
    [SerializeField] GameObject advancedCombos;
    [SerializeField] GameObject pauseMenu;

    private GameManager gameManager;

    protected override void Init()
    {
        buttonMap = new Button[,] { { basicCombosButton, advancedCombosButton, backButton } };
        gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
    }

    protected override void ResetMenu()
    {
        ShowBasicCombos();
    }

    private void OnEnable()
    {
        ResetMenu();
        if (gameManager.IsPlayerOnePaused())
        {
            LoadMenu();
        }
        else
        {
            menuNavigation.LoadMenu(this, buttonMap, 0, 0, 0, 0);
            menuNavigation.DeactivateMenu();
        }
    }

    public void ShowBasicCombos()
    {
        basicCombos.SetActive(true);
        advancedCombos.SetActive(false);
    }

    public void ShowAdvancedCombos()
    {
        basicCombos.SetActive(false);
        advancedCombos.SetActive(true);
        int fighterNumber = 0;
        if (gameManager.IsPlayerOnePaused())
        {
            fighterNumber = (int) GameData.GetFighterOneCharacter() + 1;
        }
        else
        {
            fighterNumber = (int)GameData.GetFighterTwoCharacter() + 1;
        }
        for (int i = 1; i < advancedCombos.transform.childCount; i++) 
        {
            if (i == fighterNumber)
            {
                advancedCombos.transform.GetChild(i).gameObject.SetActive(true);    //Activate correct fighter combo body
            }
            else
            {
                advancedCombos.transform.GetChild(i).gameObject.SetActive(false);   //Deactivate all other combo bodies
            }
        }
    }

    public void Back()
    {
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
