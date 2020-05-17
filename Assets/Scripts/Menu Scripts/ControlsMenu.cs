﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{

    [SerializeField] Text controllerControlsText;

    private void OnEnable()
    {
        ShowControllerControls();
    }

    public void ShowControllerControls()
    {
        controllerControlsText.enabled = true;
    }
    {
        controllerControlsText.enabled = false;
    }
}