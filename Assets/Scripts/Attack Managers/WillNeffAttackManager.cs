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
        attackManager.QueueUpAttack("Lunge Punch", 5, AttackType.Flinch);
        return pState;
    }

    public override ComboState Kick()
    {
        return this;
    }

    public override ComboState RangedAttack()
    {
        return this;
    }

    public override ComboState SpecialAttack()
    {
        if (attackManager.HasEnoughSpecial(10))
        {
            attackManager.QueueUpAttack("Lunge Punch", 15, AttackType.KnockBack, 10);
            return endComboState;
        }
        return this;
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

        attackManager.QueueUpAttack("Lunge Punch", 5, AttackType.Flinch);
        return ppState;
    }

    public override ComboState Kick()
    {
        return endComboState;
    }

    public override ComboState RangedAttack()
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
        attackManager.QueueUpAttack("Punch", 10, AttackType.KnockBack);
        return endComboState;
    }

    public override ComboState Kick()
    {
        return endComboState;
    }

    public override ComboState RangedAttack()
    {
        return endComboState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}