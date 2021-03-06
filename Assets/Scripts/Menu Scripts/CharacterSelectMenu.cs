﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSelectMenu : BaseMenuScript
{

    [SerializeField] Text fighterOneText;    [SerializeField] Text fighterTwoText;    [SerializeField] Image fighterOneDisplay;    [SerializeField] Image fighterTwoDisplay;
    [SerializeField] GameObject cpuDifficultyMenu;
    [SerializeField] GameObject confirmationMessage;

    [SerializeField] Button willNeffButton;
    [SerializeField] Button hasanAbiButton;
    [SerializeField] Button nesuaButton;
    [SerializeField] Button redPlayerButton;
    [SerializeField] Button mainMenuButton;

    private Color[] colors = { Color.black, Color.grey, Color.magenta, Color.red };    private int playerCount = 0;    private bool fighterOneSelected;
    private bool fighterTwoSelected;

    private int playerOneCharacterPosition; //Will have to change logic for this
    private int playerTwoCharacterPosition; //if characters span more than one row

    private void OnEnable()
    {
        playerOneCharacterPosition = 0;
        playerTwoCharacterPosition = 1;
        ResetAndLoadMenu();
    }

    protected override void Init()
    {
        buttonMap = new Button[,] { { willNeffButton, hasanAbiButton, nesuaButton, redPlayerButton},
                                    { mainMenuButton, null,           null,        null           } };
    }

    private void LateUpdate()
    {
        if (playerCount == 1)
        {
            if (!fighterOneSelected)
            {
                HighlightPlayerOneCharacter(menuNavigation.GetX());
                DeselectCharacter(false);
            }
            else if (!fighterTwoSelected)
            {
                HighlightPlayerTwoCharacter(menuNavigation.GetX());
            }
        }
        else
        {
            if (!fighterOneSelected)
            {
                HighlightPlayerOneCharacter(menuNavigation.GetX());
            }
            if (!fighterTwoSelected)
            {
                HighlightPlayerTwoCharacter(menuNavigation.GetPlayerTwoX());
            }
        }
        
    }

    public override void Cancel(bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            if (fighterTwoSelected && playerCount == 1)
            {
                DeselectCharacter(false);
                menuNavigation.LoadMenu(this, buttonMap, playerTwoCharacterPosition, 0);
                menuNavigation.SwapSelector();
            }
            else if (fighterOneSelected)
            {
                DeselectCharacter(true);
                menuNavigation.UnlockPlayerOneSelection();
                if (playerCount == 1)
                {
                    menuNavigation.SwapSelector();
                    menuNavigation.LoadMenu(this, buttonMap, playerOneCharacterPosition, 0);
                }
                else
                {
                    menuNavigation.SwapSelectedSprite();
                }
            }
            else
            {
                mainMenuButton.onClick.Invoke();
            }
        }
        else
        {
            if (fighterTwoSelected)
            {
                DeselectCharacter(false);
                menuNavigation.UnlockPlayerTwoSelection();
                menuNavigation.SwapSelectedSpritePlayerTwo();
            }
            else
            {
                mainMenuButton.onClick.Invoke();
            }
        }
    }

    public void ResetAndLoadMenu()
    {
        playerCount = GameData.GetPlayers();
        fighterOneSelected = false;
        fighterTwoSelected = false;
        fighterOneText.text = "";        fighterTwoText.text = "";        fighterOneDisplay.color = Color.white;        fighterTwoDisplay.color = Color.white;        confirmationMessage.SetActive(false);
        if (playerCount == 1)
        {
            LoadMenu();
        }
        else if (playerCount == 2)
        {
            menuNavigation.LoadMenu(this, buttonMap, playerOneCharacterPosition, 0, playerTwoCharacterPosition, 0);
        }
    }

    public void HighlightPlayerOneCharacter(int fighterNumber)
    {
        characters character = (characters)fighterNumber;
        fighterOneText.text = "" + character;
        fighterOneDisplay.color = colors[fighterNumber];
    }

    public void HighlightPlayerTwoCharacter(int fighterNumber)
    {
        characters character = (characters)fighterNumber;
        fighterTwoText.text = "" + character;
        fighterTwoDisplay.color = colors[fighterNumber];
    }

    public void SelectCharacter(int fighterNumber)    {        characters character = (characters)fighterNumber;

        if (playerCount == 1)
        {
            if (!fighterOneSelected)
            {
                fighterOneSelected = true;
                playerOneCharacterPosition = fighterNumber;
                GameData.SetFighterOneCharacter(character);

            }
            else if (!fighterTwoSelected)
            {
                fighterTwoSelected = true;
                playerTwoCharacterPosition = fighterNumber;
                GameData.SetFighterTwoCharacter(character);
            }
            menuNavigation.SwapSelector();
        }
        else if (playerCount == 2)
        {
            if (GameData.GetPressedButtonPlayerOne())
            {
                fighterOneSelected = true;
                playerOneCharacterPosition = fighterNumber;
                GameData.SetFighterOneCharacter(character);
                menuNavigation.LockPlayerOneSelection();
                menuNavigation.SwapSelectedSprite();
            }
            else
            {
                fighterTwoSelected = true;
                playerTwoCharacterPosition = fighterNumber;
                GameData.SetFighterTwoCharacter(character);
                menuNavigation.LockPlayerTwoSelection();
                menuNavigation.SwapSelectedSpritePlayerTwo();
            }
        }        if (fighterOneSelected && fighterTwoSelected)
        {            PromptConfirmation();        }    }

    public void PromptConfirmation()    {
        if (playerCount == 1)
        {
            cpuDifficultyMenu.SetActive(true);
        }
        else
        {
            confirmationMessage.SetActive(true);
        }    }    public void DeselectCharacter(bool fighterOne)    {        if (fighterOne)
        {
            fighterOneText.text = "";
            fighterOneDisplay.color = Color.white;
            fighterOneSelected = false;
        }        else
        {
            fighterTwoText.text = "";
            fighterTwoDisplay.color = Color.white;
            fighterTwoSelected = false;
        }    }
}

public enum characters
{
    WillNeff,
    HasanAbi,
    Nesua,
    red
}