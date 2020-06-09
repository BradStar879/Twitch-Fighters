using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ConfirmationMenu : BaseMenuScript
{
    [SerializeField] Button roundSelector;
    [SerializeField] Button readyButton;
    [SerializeField] CharacterSelectMenu characterSelectMenu;
    [SerializeField] GameObject difficultyMenu;

    private ControllerInput controllerInput;
    private Text roundText;
    private int numberOfRounds;
    private float repeatDelay = .2f;
    private float repeatDelayCount = 0f;

    protected override void Init()
    {
        buttonMap = new Button[,] { { roundSelector },
                                    { readyButton   } };
        controllerInput = ControllerManager.GetControllerInput(1);
        roundText = roundSelector.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (repeatDelayCount > 0f)
        {
            repeatDelayCount -= Time.deltaTime;
        }
        else if (menuNavigation.IsButtonSelected(roundSelector))
        {
            if (controllerInput.GetXAxisLeft())
            {
                numberOfRounds--;
                if (numberOfRounds < 1)
                {
                    numberOfRounds = 3;
                }
                UpdateRoundText();
            }
            else if (controllerInput.GetXAxisRight())
            {
                numberOfRounds++;
                if (numberOfRounds > 3)
                {
                    numberOfRounds = 1;
                }
                UpdateRoundText();
            }
        }

    }

    public void LoadFightScene()
    {
        GameData.SetNumberOfRounds(numberOfRounds);
        SceneManager.LoadScene("Fight Scene");
    }

    protected override void ResetMenu()
    {
        numberOfRounds = 3;
        UpdateRoundText();
    }

    public override void Cancel(bool isPlayerOne)
    {
        if (GameData.GetPlayers() == 1)
        {
            difficultyMenu.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            characterSelectMenu.ResetAndLoadMenu();
            gameObject.SetActive(false);
        }
    }

    private void UpdateRoundText()
    {
        roundText.text = "" + numberOfRounds;
        repeatDelayCount = repeatDelay;
    }
}
