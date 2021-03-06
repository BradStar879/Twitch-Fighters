﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput
{
    private int playerNumber = 1;   //Player number starts at 1, not 0
    private InputState inputState;
    private bool usingController;

    private void Start() { }

    public void Init(int playerNumber)
    {
        this.playerNumber = playerNumber;
        inputState = new KeyboardState(playerNumber);
    }

    public bool IsUsingController()
    {
        return usingController;
    }

    public void ActivateKeyboardInput()
    {
        inputState = new KeyboardState(playerNumber);
        usingController = false;
    }

    public void ActivateXboxControllerInput(int inputSpot)
    {
        inputState = new XboxControllerState(inputSpot);
        usingController = true;
    }

    public void ActivatePlayStationControllerInput(int inputSpot)
    {
        inputState = new PlayStationControllerState(inputSpot);
        usingController = true;
    }

    public float GetXAxis()
    {
        return inputState.GetXAxis();
    }

    public bool GetXAxisLeft()
    {
        return inputState.GetXAxisLeft();
    }

    public bool GetXAxisRight()
    {
        return inputState.GetXAxisRight();
    }

    public float GetYAxis()
    {
        return inputState.GetYAxis();
    }

    public bool GetYAxisUp()
    {
        return inputState.GetYAxisUp();
    }

    public bool GetYAxisDown()
    {
        return inputState.GetYAxisDown();
    }

    public bool GetBottomActionButton()
    {
        return inputState.GetBottomActionButton();
    }

    public bool GetBottomActionButtonDown()
    {
        return inputState.GetBottomActionButtonDown();
    }

    public bool GetLeftActionButton()
    {
        return inputState.GetLeftActionButton();
    }

    public bool GetLeftActionButtonDown()
    {
        return inputState.GetLeftActionButtonDown();
    }

    public bool GetTopActionButton()
    {
        return inputState.GetTopActionButton();
    }

    public bool GetTopActionButtonDown()
    {
        return inputState.GetTopActionButtonDown();
    }

    public bool GetRightActionButton()
    {
        return inputState.GetRightActionButton();
    }

    public bool GetRightActionButtonDown()
    {
        return inputState.GetRightActionButtonDown();
    }

    public bool GetLeftBumperDown()
    {
        return inputState.GetLeftBumperDown();
    }

    public bool GetRightBumper()
    {
        return inputState.GetRightBumper();
    }

    public bool GetRightTrigger()
    {
        return inputState.GetRightTrigger();
    }

    public bool GetLeftTrigger()
    {
        return inputState.GetLeftTrigger();
    }

    public bool GetStartButton()
    {
        return inputState.GetStartButton();
    }

    public bool GetStartButtonDown()
    {
        return inputState.GetStartButtonDown();
    }
}
