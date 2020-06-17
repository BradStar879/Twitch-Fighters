using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XboxControllerState : InputState
{
    public XboxControllerState(int inputSpot)
    {
        this.inputSpot = inputSpot;
    }

    public override float GetXAxis()
    {
        return Input.GetAxis("Controller" + inputSpot + "_Axis1");
    }

    public override float GetYAxis()
    {
        return Input.GetAxis("Controller" + inputSpot + "_Axis2");
    }

    public override float GetSecondaryXAxis()
    {
        return Input.GetAxis("Controller" + inputSpot + "_Axis4");
    }

    public override float GetSecondaryYAxis()
    {
        return Input.GetAxis("Controller" + inputSpot + "_Axis5");
    }

    public override bool GetBottomActionButton()
    {
        return Input.GetButton("Controller" + inputSpot + "_Button0");
    }

    public override bool GetBottomActionButtonDown()
    {
        return Input.GetButtonDown("Controller" + inputSpot + "_Button0");
    }

    public override bool GetLeftActionButton()
    {
        return Input.GetButton("Controller" + inputSpot + "_Button2");
    }

    public override bool GetLeftActionButtonDown()
    {
        return Input.GetButtonDown("Controller" + inputSpot + "_Button2");
    }

    public override bool GetTopActionButton()
    {
        return Input.GetButton("Controller" + inputSpot + "_Button3");
    }

    public override bool GetTopActionButtonDown()
    {
        return Input.GetButtonDown("Controller" + inputSpot + "_Button3");
    }

    public override bool GetRightActionButton()
    {
        return Input.GetButton("Controller" + inputSpot + "_Button1");
    }

    public override bool GetRightActionButtonDown()
    {
        return Input.GetButtonDown("Controller" + inputSpot + "_Button1");
    }

    public override bool GetLeftBumper()
    {
        return Input.GetButton("Controller" + inputSpot + "_Button4");
    }

    public override bool GetLeftBumperDown()
    {
        return Input.GetButtonDown("Controller" + inputSpot + "_Button4");
    }

    public override bool GetRightBumper()
    {
        return Input.GetButton("Controller" + inputSpot + "_Button5");
    }

    public override bool GetRightBumperDown()
    {
        return Input.GetButtonDown("Controller" + inputSpot + "_Button5");
    }

    public override bool GetRightTrigger()
    {
        return Input.GetAxis("Controller" + inputSpot + "_Axis3") >= axisMinimum;
    }

    public override bool GetLeftTrigger()
    {
        return Input.GetAxis("Controller" + inputSpot + "_Axis3") <= -axisMinimum;
    }

    public override bool GetStartButton()
    {
        return Input.GetButton("Controller" + inputSpot + "_Button7");
    }

    public override bool GetStartButtonDown()
    {
        return Input.GetButtonDown("Controller" + inputSpot + "_Button7");
    }
}
