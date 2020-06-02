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

    private Color[] colors = { Color.black, Color.grey, Color.magenta, Color.red };    private int playerCount = 0;    private bool fighterOneSelected;
    private bool fighterTwoSelected;

    private void OnEnable()
    {
        ResetMenu();
        if (playerCount == 1)
        {
            LoadMenu();
        } 
        else if (playerCount == 2)
        {
            menuNavigation.LoadMenu(this, buttonMap, 0, 0, 1, 0);
        }
    }

    protected override void Init()
    {
        buttonMap = new Button[,] { { willNeffButton, hasanAbiButton, nesuaButton, redPlayerButton},
                                    { mainMenuButton, null,           null,        null           } };
        confirmationButtonMap = new Button[,] { {yesButton, noButton} };
    }

    public override void Cancel(bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            if (fighterOneSelected)
            {
                DeselectCharacter(true);
                menuNavigation.UnlockPlayerOneSelection();
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
            }
            else
            {
                mainMenuButton.onClick.Invoke();
            }
        }
    }

    protected override void ResetMenu()
    {
        playerCount = GameData.GetPlayers();
        fighterOneSelected = false;
        fighterTwoSelected = false;
        fighterOneText.text = "";        fighterTwoText.text = "";        fighterOneDisplay.color = Color.white;        fighterTwoDisplay.color = Color.white;        confirmationMessage.SetActive(false);
    }

    public void SelectCharacter(int fighterNumber)    {        characters character = (characters)fighterNumber;

        if (playerCount == 1)
        {
            if (!fighterOneSelected)
            {
                fighterOneText.text = "" + character;
                fighterOneDisplay.color = colors[fighterNumber];
                fighterOneSelected = true;
                GameData.SetFighterOneCharacter(character);

            }
            else if (!fighterTwoSelected)
            {
                fighterTwoText.text = "" + character;
                fighterTwoDisplay.color = colors[fighterNumber];
                fighterTwoSelected = true;
                GameData.SetFighterTwoCharacter(character);
            }
        }
        else if (playerCount == 2)
        {
            if (GameData.GetPressedButtonPlayerOne())
            {
                fighterOneText.text = "" + character;
                fighterOneDisplay.color = colors[fighterNumber];
                fighterOneSelected = true;
                GameData.SetFighterOneCharacter(character);
                menuNavigation.LockPlayerOneSelection();
            }
            else
            {
                fighterTwoText.text = "" + character;
                fighterTwoDisplay.color = colors[fighterNumber];
                fighterTwoSelected = true;
                GameData.SetFighterTwoCharacter(character);
                menuNavigation.LockPlayerTwoSelection();
            }
        }        if (fighterOneSelected && fighterTwoSelected)
        {            PromptConfirmation();        }    }

    public void PromptConfirmation()    {
        menuNavigation.DeselectButton();        confirmationMessage.SetActive(true);
        menuNavigation.LoadMenu(this, confirmationButtonMap, 0, 0);    }    public void PromptConfirm()    {
        SceneManager.LoadScene("Fight Scene");    }    public void PromptCancel()
    {
        OnEnable();
    }    public void DeselectCharacter(bool fighterOne)    {        if (fighterOne)
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