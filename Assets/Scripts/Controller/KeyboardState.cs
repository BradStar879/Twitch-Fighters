using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardState : InputState
{
    public KeyboardState(int inputSpot)
    {
        this.inputSpot = inputSpot;
    }

    public override float GetXAxis()
    {
        return Input.GetAxis("Keyboard" + inputSpot + "_xAxis");
    }

    public override float GetYAxis()
    {
        return Input.GetAxis("Keyboard" + inputSpot + "_yAxis");
    }

    public override float GetSecondaryXAxis()
    {
        return Input.GetAxis("Keyboard" + inputSpot + "_SecondaryXAxis");
    }

    public override float GetSecondaryYAxis()
    {
        return Input.GetAxis("Keyboard" + inputSpot + "_SecondaryYAxis");
    }

    public override bool GetBottomActionButton()
    {
        return Input.GetButton("Keyboard" + inputSpot + "_BottomAction");
    }

    public override bool GetBottomActionButtonDown()
    {
        return Input.GetButtonDown("Keyboard" + inputSpot + "_BottomAction");
    }

    public override bool GetLeftActionButton()
    {
        return Input.GetButton("Keyboard" + inputSpot + "_LeftAction");
    }

    public override bool GetLeftActionButtonDown()
    {
        return Input.GetButtonDown("Keyboard" + inputSpot + "_LeftAction");
    }

    public override bool GetTopActionButton()
    {
        return Input.GetButton("Keyboard" + inputSpot + "_TopAction");
    }

    public override bool GetTopActionButtonDown()
    {
        return Input.GetButtonDown("Keyboard" + inputSpot + "_TopAction");
    }

    public override bool GetRightActionButton()
    {
        return Input.GetButton("Keyboard" + inputSpot + "_RightAction");
    }

    public override bool GetRightActionButtonDown()
    {
        return Input.GetButtonDown("Keyboard" + inputSpot + "_RightAction");
    }

    public override bool GetLeftBumper()
    {
        return Input.GetButton("Keyboard" + inputSpot + "_LeftBumper");
    }

    public override bool GetLeftBumperDown()
    {
        return Input.GetButtonDown("Keyboard" + inputSpot + "_LeftBumper");
    }

    public override bool GetRightBumper()
    {
        return Input.GetButton("Keyboard" + inputSpot + "_RightBumper");
    }

    public override bool GetRightBumperDown()
    {
        return Input.GetButtonDown("Keyboard" + inputSpot + "_RightBumper");
    }

    public override bool GetRightTrigger()
    {
        return Input.GetAxis("Keyboard" + inputSpot + "_Triggers") >= axisMinimum;
    }

    public override bool GetLeftTrigger()
    {
        return Input.GetAxis("Keyboard" + inputSpot + "_Triggers") <= -axisMinimum;
    }

    public override bool GetStartButton()
    {
        return Input.GetButton("Keyboard" + inputSpot + "_Start");
    }

    public override bool GetStartButtonDown()
    {
        return Input.GetButtonDown("Keyboard" + inputSpot + "_Start");
    }
}
