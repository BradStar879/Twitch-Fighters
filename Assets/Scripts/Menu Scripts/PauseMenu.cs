using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenuScript
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button combosButton;
    [SerializeField] Button mainMenuButton;

    [SerializeField] GameObject comboMenu;
    private GameManager gameManager;

    protected override void Init()
    {
        buttonMap = new Button[,] { { resumeButton, combosButton, mainMenuButton } };
        gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
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

    public void DisplayCombos()
    {
        comboMenu.SetActive(true);
        gameObject.SetActive(false);
    }

}