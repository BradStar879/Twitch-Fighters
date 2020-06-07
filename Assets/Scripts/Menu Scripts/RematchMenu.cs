using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RematchMenu : BaseMenuScript
{
    [SerializeField] Button rematchButton;
    [SerializeField] Button newFightersButton;
    [SerializeField] Button mainMenuButton;
    private GameManager gameManager;

    protected override void Init()
    {
        buttonMap = new Button[,] { { rematchButton, newFightersButton, mainMenuButton } };
        gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        ResetMenu();
        if (GameData.GetPlayers() == 1 || !gameManager.IsPlayerOneWinner()) //Player 1 has control if 1 player game or is loser
        {
            LoadMenu();
        }
        else    //Player 2 has control if 2 player game and is loser
        {
            menuNavigation.LoadMenu(this, buttonMap, 0, 0, 0, 0);
            menuNavigation.DeactivateMenu();
        }
    }

}
