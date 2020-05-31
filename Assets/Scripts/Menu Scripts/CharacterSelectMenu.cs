using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;using UnityEngine.SceneManagement;

public class CharacterSelectMenu : BaseMenuScript
{

    [SerializeField] Text fighterOneText;    [SerializeField] Text fighterTwoText;    [SerializeField] Image fighterOneDisplay;    [SerializeField] Image fighterTwoDisplay;
    [SerializeField] GameObject confirmationMessage;

    [SerializeField] Button willNeffButton;
    [SerializeField] Button hasanAbiButton;
    [SerializeField] Button nesuaButton;
    [SerializeField] Button redPlayerButton;
    [SerializeField] Button mainMenuButton;

    private Button[,] confirmationButtonMap;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    private Color[] colors = { Color.black, Color.grey, Color.magenta, Color.red };    private int playerCount = 0;    private int playersSelected = 0;

    protected override void Init()
    {
        buttonMap = new Button[,] { { willNeffButton, hasanAbiButton, nesuaButton, redPlayerButton},
                                    { mainMenuButton, null,           null,        null           } };
        confirmationButtonMap = new Button[,] { {yesButton, noButton} };
    }

    protected override void ResetMenu()
    {
        playerCount = GameData.GetPlayers();
        playersSelected = 0;
        fighterOneText.text = "";        fighterTwoText.text = "";        fighterOneDisplay.color = Color.white;        fighterTwoDisplay.color = Color.white;        confirmationMessage.SetActive(false);
    }

    public void SelectCharacter(int fighterNumber)    {        characters character = (characters)fighterNumber;        playersSelected++;        if (playersSelected == 1)        {            fighterOneText.text = "" + character;            fighterOneDisplay.color = colors[fighterNumber];            GameData.SetFighterOneCharacter(character);        }
        else if (playersSelected == 2)        {            fighterTwoText.text = "" + character;            fighterTwoDisplay.color = colors[fighterNumber];            GameData.SetFighterTwoCharacter(character);        }        if (playersSelected == playerCount)
        {            PromptConfirmation();        }    }

    public void PromptConfirmation()    {
        menuNavigation.DeselectButton();        confirmationMessage.SetActive(true);
        menuNavigation.LoadMenu(confirmationButtonMap, 0, 0);    }    public void Confirm()    {
        SceneManager.LoadScene("Fight Scene");    }    public void Cancel()
    {
        LoadMenu();
        ResetMenu();
    }    public void DeselectCharacter()    {        if (playersSelected == 1)
        {
            fighterOneText.text = "";
            fighterOneDisplay.color = Color.white;
        }        else if (playersSelected == 2)
        {
            fighterTwoText.text = "";
            fighterTwoDisplay.color = Color.white;
        }        playersSelected--;    }
}

public enum characters
{
    WillNeff,
    HasanAbi,
    Nesua,
    red
}