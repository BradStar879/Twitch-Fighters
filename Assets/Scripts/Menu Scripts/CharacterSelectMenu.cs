﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{

    [SerializeField] Text fighterOneText;
    [SerializeField] GameObject confirmationMessage;
    [SerializeField] Button[] nonpromptButtons;

    private Color[] colors = { Color.black, Color.grey, Color.magenta, Color.red };

    private void OnEnable()
    {
        StartCharacterSelection();
    }

    public void StartCharacterSelection()
    {
        playerCount = GameData.GetPlayers();
        playersSelected = 0;
        fighterOneText.text = "";
    }

    public void SelectCharacter(int fighterNumber)
        else if (playersSelected == 2)
        {

    public void PromptConfirmation()
    {
        foreach (Button button in nonpromptButtons)
        {
            button.interactable = false;
        }
    }
    {
        foreach (Button button in nonpromptButtons)
        {
            button.interactable = true;
        }
    }
        SceneManager.LoadScene("Fight Scene");
    {
        EnableNonpromptButtons();
        StartCharacterSelection();
        GetComponent<DefaultButtonSelector>().SelectDefaultButton();
    }
        {
            fighterOneText.text = "";
            fighterOneDisplay.color = Color.white;
        }
        {
            fighterTwoText.text = "";
            fighterTwoDisplay.color = Color.white;
        }
}

public enum characters
{
    WillNeff,
    HasanAbi,
    Nesua,
    red
}