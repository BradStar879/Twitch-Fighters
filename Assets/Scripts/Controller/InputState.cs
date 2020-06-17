using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputState
{
    protected int inputSpot;
    protected readonly float axisMinimum = .7f;

    public abstract float GetXAxis();

    public bool GetXAxisLeft()
    {
        return GetXAxis() <= -axisMinimum;
    }

    public bool GetXAxisRight()
    {
        return GetXAxis() >= axisMinimum;
    }

    public abstract float GetYAxis();

    public bool GetYAxisDown()
    {
        return GetYAxis() <= -axisMinimum;
    }

    public bool GetYAxisUp()
    {
        return GetYAxis() >= axisMinimum;
    }

    public abstract float GetSecondaryXAxis();

    public bool GetSecondaryXAxisLeft()
    {
        return GetSecondaryXAxis() <= -axisMinimum;
    }

    public bool GetSecondaryXAxisRight()
    {
        return GetSecondaryXAxis() >= axisMinimum;
    }

    public abstract float GetSecondaryYAxis();

    public bool GetSecondaryYAxisDown()
    {
        return GetSecondaryYAxis() <= -axisMinimum;
    }

    public bool GetSecondaryYAxisUp()
    {
        return GetSecondaryYAxis() >= axisMinimum;
    }

    public abstract bool GetBottomActionButton();

    public abstract bool GetBottomActionButtonDown();

    public abstract bool GetLeftActionButton();

    public abstract bool GetLeftActionButtonDown();

    public abstract bool GetTopActionButton();

    public abstract bool GetTopActionButtonDown();

    public abstract bool GetRightActionButton();

    public abstract bool GetRightActionButtonDown();

    public abstract bool GetLeftBumper();

    public abstract bool GetLeftBumperDown();

    public abstract bool GetRightBumper();

    public abstract bool GetRightBumperDown();

    public abstract bool GetRightTrigger();

    public abstract bool GetLeftTrigger();

    public abstract bool GetStartButton();

    public abstract bool GetStartButtonDown();

}
