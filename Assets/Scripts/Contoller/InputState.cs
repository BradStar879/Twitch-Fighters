using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState
{
    private string inputType;
    private int inputSpot;

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
        return GetXAxis() <= -.8f;
    }

    public bool GetXAxisRight()
    {
        return GetXAxis() >= .8f;
    }

    public float GetYAxis()
    {
        return Input.GetAxis(inputType + inputSpot + "_yAxis");
    }

    public bool GetYAxisDown()
    {
        return GetYAxis() <= -.8f;
    }

    public bool GetYAxisUp()
    {
        return GetYAxis() >= .8f;
    }

    public float GetSecondaryXAxis()
    {
        return Input.GetAxis(inputType + inputSpot + "_SecondaryXAxis");
    }

    public bool GetSecondaryXAxisLeft()
    {
        return GetSecondaryXAxis() <= -.8f;
    }

    public bool GetSecondaryXAxisRight()
    {
        return GetSecondaryXAxis() >= .8f;
    }

    public float GetSecondaryYAxis()
    {
        return Input.GetAxis(inputType + inputSpot + "_SecondaryYAxis");
    }

    public bool GetSecondaryYAxisDown()
    {
        return GetSecondaryYAxis() <= -.8f;
    }

    public bool GetSecondaryYAxisUp()
    {
        return GetSecondaryYAxis() >= .8f;
    }

    public bool GetBottomActionButton()
    {
        return Input.GetButton(inputType + inputSpot + "_BottomAction");
    }

    public bool GetBottomActionButtonDown()
    {
        return Input.GetButtonDown(inputType + inputSpot + "_BottomAction");
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

    public float GetRightTrigger()
    {
        return Input.GetAxis(inputType + inputSpot + "_RightTrigger");
    }

    public bool GetRightTriggerDown()
    {
        return GetRightTrigger() > .8f;
    }

    public float GetLeftTrigger()
    {
        return Input.GetAxis(inputType + inputSpot + "_LeftTrigger");
    }

    public bool GetLeftTriggerDown()
    {
        return GetLeftTrigger() > .8f;
    }
}
