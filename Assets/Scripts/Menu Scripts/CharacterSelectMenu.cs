using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;using UnityEngine.SceneManagement;

public class CharacterSelectMenu : MonoBehaviour
{

    [SerializeField] Text fighterOneText;    [SerializeField] Text fighterTwoText;    [SerializeField] Image fighterOneDisplay;    [SerializeField] Image fighterTwoDisplay;
    [SerializeField] GameObject confirmationMessage;
    [SerializeField] Button[] nonpromptButtons;

    private Color[] colors = { Color.black, Color.grey, Color.magenta, Color.red };    private int playerCount = 0;    private int playersSelected = 0;

    private void OnEnable()
    {
        StartCharacterSelection();
    }

    public void StartCharacterSelection()
    {
        playerCount = GameData.GetPlayers();
        playersSelected = 0;
        fighterOneText.text = "";        fighterTwoText.text = "";        fighterOneDisplay.color = Color.white;        fighterTwoDisplay.color = Color.white;        confirmationMessage.SetActive(false);
    }

    public void SelectCharacter(int fighterNumber)    {        characters character = (characters)fighterNumber;        playersSelected++;        if (playersSelected == 1)        {            fighterOneText.text = "" + character;            fighterOneDisplay.color = colors[fighterNumber];            GameData.SetFighterOneCharacter(character);        }
        else if (playersSelected == 2)        {            fighterTwoText.text = "" + character;            fighterTwoDisplay.color = colors[fighterNumber];            GameData.SetFighterTwoCharacter(character);        }        if (playersSelected == playerCount)
        {            PromptConfirmation();        }    }

    public void PromptConfirmation()    {        DisableNonpromptButtons();        confirmationMessage.SetActive(true);    }    private void DisableNonpromptButtons()
    {
        foreach (Button button in nonpromptButtons)
        {
            button.interactable = false;
        }
    }    private void EnableNonpromptButtons()
    {
        foreach (Button button in nonpromptButtons)
        {
            button.interactable = true;
        }
    }    public void Confirm()    {
        SceneManager.LoadScene("Fight Scene");    }    public void Cancel()
    {
        EnableNonpromptButtons();
        StartCharacterSelection();
        GetComponent<DefaultButtonSelector>().SelectDefaultButton();
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