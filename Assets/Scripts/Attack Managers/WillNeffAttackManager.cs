using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillNeffAttackManager : AttackManager
{
    private WillNeffComboStateDefault defaultState;
    private WillNeffComboStateP pState;
    private WillNeffComboStatePP ppState;

    public override void Init()
    {
        base.Init();
        ppState = new WillNeffComboStatePP(anim);
        pState = new WillNeffComboStateP(anim, ppState);
        defaultState = new WillNeffComboStateDefault(anim, pState);
        ResetCombo();
    }

    public override ComboState GetDefaultState()
    {
        return defaultState;
    }
}

public class WillNeffComboStateDefault : ComboState
{
    private WillNeffComboStateP pState;

    public WillNeffComboStateDefault(Animator anim, WillNeffComboStateP pState) : base(anim)
    {
        this.pState = pState;
    }

    public override ComboState Punch()
    {
        anim.Play("Lunge Punch");
        return pState;
    }

    public override ComboState Kick()
    {
        return endComboState;
    }

    public override ComboState RangeAttack()
    {
        return endComboState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}

public class WillNeffComboStateP : ComboState
{
    private WillNeffComboStatePP ppState;

    public WillNeffComboStateP(Animator anim, WillNeffComboStatePP ppState) : base(anim)
    {
        this.ppState = ppState;
    }

    public override ComboState Punch()
    {
        anim.Rebind();
        anim.Play("Lunge Punch");
        return ppState;
    }

    public override ComboState Kick()
    {
        return endComboState;
    }

    public override ComboState RangeAttack()
    {
        return endComboState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}

public class WillNeffComboStatePP : ComboState
{
    public WillNeffComboStatePP(Animator anim) : base(anim) { }

    public override ComboState Punch()
    {
        anim.Rebind();
        anim.Play("Punch");
        return endComboState;
    }

    public override ComboState Kick()
    {
        return endComboState;
    }

    public override ComboState RangeAttack()
    {
        return endComboState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}