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
        ppState = new WillNeffComboStatePP(this);
        pState = new WillNeffComboStateP(this, ppState);
        defaultState = new WillNeffComboStateDefault(this, pState);
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

    public WillNeffComboStateDefault(AttackManager attackManager, WillNeffComboStateP pState) : base(attackManager)
    {
        this.pState = pState;
    }

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Lunge Punch", 5);
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

    public WillNeffComboStateP(AttackManager attackManager, WillNeffComboStatePP ppState) : base(attackManager)
    {
        this.ppState = ppState;
    }

    public override ComboState Punch()
    {

        attackManager.QueueUpAttack("Lunge Punch", 5);
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
    public WillNeffComboStatePP(AttackManager attackManager) : base(attackManager) { }

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Punch", 10);
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