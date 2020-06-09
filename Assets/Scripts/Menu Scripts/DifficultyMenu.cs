using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyMenu : BaseMenuScript
{
    [SerializeField] Button easyButton;
    [SerializeField] Button mediumButton;
    [SerializeField] Button hardButton;
    [SerializeField] CharacterSelectMenu characterSelectMenu;
    [SerializeField] GameObject confirmationMessage;
    private GameManager gameManager;

    protected override void Init()
    {
        buttonMap = new Button[,] { { easyButton },
                                    { mediumButton },
                                    { hardButton } };
        gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
    }

    public void EasyCpu()
    {
        GameData.SetComputerDifficulty(Difficulty.Easy);
        confirmationMessage.SetActive(true);
        gameObject.SetActive(false);
    }

    public void MediumCpu()
    {
        GameData.SetComputerDifficulty(Difficulty.Medium);
        confirmationMessage.SetActive(true);
        gameObject.SetActive(false);
    }

    public void HardCpu()
    {
        GameData.SetComputerDifficulty(Difficulty.Hard);
        confirmationMessage.SetActive(true);
        gameObject.SetActive(false);
    }

    public override void Cancel(bool isPlayerOne)
    {
        characterSelectMenu.ResetAndLoadMenu();
        gameObject.SetActive(false);
    }
}
