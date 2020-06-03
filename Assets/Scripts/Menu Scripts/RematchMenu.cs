using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RematchMenu : BaseMenuScript
{
    [SerializeField] Button rematchButton;
    [SerializeField] Button newFightersButton;
    [SerializeField] Button mainMenuButton;

    protected override void Init()
    {
        buttonMap = new Button[,] { { rematchButton, newFightersButton, mainMenuButton } };
    }

}
