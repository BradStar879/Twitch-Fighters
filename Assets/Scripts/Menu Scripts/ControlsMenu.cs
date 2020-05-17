using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{

    [SerializeField] Text controllerControlsText;    [SerializeField] GameObject keyboardControls;

    private void OnEnable()
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
