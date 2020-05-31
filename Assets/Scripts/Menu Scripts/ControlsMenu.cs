using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : BaseMenuScript
{

    [SerializeField] Text controllerControlsText;    [SerializeField] GameObject keyboardControls;
    [SerializeField] Button controllerControlsButton;
    [SerializeField] Button keyboardControlsButton;
    [SerializeField] Button mainMenuButton;

    protected override void Init()
    {
        buttonMap = new Button[,] { { controllerControlsButton, keyboardControlsButton, mainMenuButton } };
    }

    protected override void ResetMenu()
    {
        ShowControllerControls();
    }

    public void ShowControllerControls()
    {
        controllerControlsText.enabled = true;        keyboardControls.SetActive(false);
    }    public void ShowKeyboardControls()
    {
        controllerControlsText.enabled = false;        keyboardControls.SetActive(true);
    }
}
