using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState
{
    private string inputType;
    private int inputSpot;
    private readonly float axisMinimum = .7f;

    public void SetAsKeyboardType(int inputSpot)
    {
        inputType = "Keyboard";
        this.inputSpot = inputSpot;
    }

    public void SetAsControllerType(int inputSpot)
    {
        inputType = "Controller";
        this.inputSpot = inputSpot;
    }

    public float GetXAxis()
    {
        return Input.GetAxis(inputType + inputSpot + "_xAxis");
    }

    public bool GetXAxisLeft()
    {
        return GetXAxis() <= -axisMinimum;
    }

    public bool GetXAxisRight()
    {
        return GetXAxis() >= axisMinimum;
    }

    public string GetXAxisString()
    {
        return inputType + inputSpot + "_xAxis";
    }

    public float GetYAxis()
    {
        return Input.GetAxis(inputType + inputSpot + "_yAxis");
    }

    public bool GetYAxisDown()
    {
        return GetYAxis() <= -axisMinimum;
    }

    public bool GetYAxisUp()
    {
        return GetYAxis() >= axisMinimum;
    }

    public string GetYAxisString()
    {
        return inputType + inputSpot + "_yAxis";
    }

    public float GetSecondaryXAxis()
    {
        return Input.GetAxis(inputType + inputSpot + "_SecondaryXAxis");
    }

    public bool GetSecondaryXAxisLeft()
    {
        return GetSecondaryXAxis() <= -axisMinimum;
    }

    public bool GetSecondaryXAxisRight()
    {
        return GetSecondaryXAxis() >= axisMinimum;
    }

    public float GetSecondaryYAxis()
    {
        return Input.GetAxis(inputType + inputSpot + "_SecondaryYAxis");
    }

    public bool GetSecondaryYAxisDown()
    {
        return GetSecondaryYAxis() <= -axisMinimum;
    }

    public bool GetSecondaryYAxisUp()
    {
        return GetSecondaryYAxis() >= axisMinimum;
    }

    public bool GetBottomActionButton()
    {
        return Input.GetButton(inputType + inputSpot + "_BottomAction");
    }

    public bool GetBottomActionButtonDown()
    {
        return Input.GetButtonDown(inputType + inputSpot + "_BottomAction");
    }

    public string GetBottomActionButtonString()
    {
        return inputType + inputSpot + "_BottomAction";
    }

    public bool GetLeftActionButton()
    {
        return Input.GetButton(inputType + inputSpot + "_LeftAction");
    }

    public bool GetLeftActionButtonDown()
    {
        return Input.GetButtonDown(inputType + inputSpot + "_LeftAction");
    }

    public bool GetTopActionButton()
    {
        return Input.GetButton(inputType + inputSpot + "_TopAction");
    }

    public bool GetTopActionButtonDown()
    {
        return Input.GetButtonDown(inputType + inputSpot + "_TopAction");
    }

    public bool GetRightActionButton()
    {
        return Input.GetButton(inputType + inputSpot + "_RightAction");
    }

    public bool GetRightActionButtonDown()
    {
        return Input.GetButtonDown(inputType + inputSpot + "_RightAction");
    }

    public string GetRightActionButtonString()
    {
        return inputType + inputSpot + "_RightAction";
    }

    public bool GetLeftBumper()
    {
        return Input.GetButton(inputType + inputSpot + "_LeftBumper");
    }

    public bool GetLeftBumperDown()
    {
        return Input.GetButtonDown(inputType + inputSpot + "_LeftBumper");
    }

    public bool GetRightBumper()
    {
        return Input.GetButton(inputType + inputSpot + "_RightBumper");
    }

    public bool GetRightBumperDown()
    {
        return Input.GetButtonDown(inputType + inputSpot + "_RightBumper");
    }

    public bool GetRightTrigger()
    {
        return Input.GetAxis(inputType + inputSpot + "_Triggers") >= axisMinimum;
    }

    public bool GetLeftTrigger()
    {
        return Input.GetAxis(inputType + inputSpot + "_Triggers") <= -axisMinimum;
    }

    public bool GetStartButton()
    {
        return Input.GetButton(inputType + inputSpot + "_Start");
    }

    public bool GetStartButtonDown()
    {
        return Input.GetButtonDown(inputType + inputSpot + "_Start");
    }

}
