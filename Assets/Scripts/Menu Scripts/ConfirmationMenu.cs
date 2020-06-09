using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ConfirmationMenu : BaseMenuScript
{
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    [SerializeField] CharacterSelectMenu characterSelectMenu;
    [SerializeField] GameObject difficultyMenu;

    protected override void Init()
    {
        buttonMap = new Button[,] { { yesButton, noButton } };
    }

    public void LoadFightScene()
    {
        SceneManager.LoadScene("Fight Scene");
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
}
