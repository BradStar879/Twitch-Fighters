using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ConfirmationMenu : BaseMenuScript
{
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    protected override void Init()
    {
        buttonMap = new Button[,] { { yesButton, noButton } };
    }

    public void LoadFightScene()
    {
        SceneManager.LoadScene("Fight Scene");
    }
}
