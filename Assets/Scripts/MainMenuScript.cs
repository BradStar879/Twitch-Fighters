﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject playerSelectMenu;
    [SerializeField] GameObject confirmationMessage;

    [SerializeField] Text fighterOneText;
    [SerializeField] Text fighterTwoText;
    [SerializeField] Image fighterOneDisplay;
    [SerializeField] Image fighterTwoDisplay;

    public enum characters
    {
        red,
        blue,
        green,
        yellow
    }

    private Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow };

    private int playerCount = 0;
    private int playersSelected = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMainMenu()
    {
        mainMenu.SetActive(true);
        playerSelectMenu.SetActive(false);
    }

    public void GoToOnePlayerSelect()
    {
        playerCount = 1;
        GoToPlayerSelect();
    }

    public void GoToTwoPlayerSelect()
    {
        playerCount = 2;
        GoToPlayerSelect();
    }

    public void SelectCharacter(int fighterNumber)
    {
        characters character = (characters) fighterNumber;
        playersSelected++;

        if (playersSelected == 1)
        {
            fighterOneText.text = "" + character;
            fighterOneDisplay.color = colors[fighterNumber];
            GameData.SetFighterOneCharacter(character);

        } 
        else if (playersSelected == 2)
        {
            fighterTwoText.text = "" + character;
            fighterTwoDisplay.color = colors[fighterNumber];
            GameData.SetFighterTwoCharacter(character);
        }

        if (playersSelected == playerCount) {
            PromptConfirmation();
        }
    }

    public void PromptConfirmation()
    {
        confirmationMessage.SetActive(true);
    }

    public void Confirm()
    {
        //Load game through scene manager
        SceneManager.LoadScene("Fight Scene");
    }

    public void Cancel()
    {
        fighterOneText.text = "";
        fighterTwoText.text = "";
        fighterOneDisplay.color = Color.white;
        fighterTwoDisplay.color = Color.white;
        confirmationMessage.SetActive(false);
        playersSelected = 0;
    }

    public void DeselectCharacter()
    {
        playersSelected--;
    }

    private void GoToPlayerSelect()
    {
        GameData.SetPlayers(playerCount);
        playersSelected = 0;
        mainMenu.SetActive(false);
        playerSelectMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
