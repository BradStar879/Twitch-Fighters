using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillNeffAttackManager : AttackManager
{
    private WillNeffComboStateDefault defaultState;
    private WillNeffComboStateP pState;
    private WillNeffComboStatePP ppState;
    private WillNeffComboStateK kState;
    private WillNeffComboStateKK kkState;

    public override void Init()
    {
        base.Init();
        cameraUltimateAnimation = "Will Neff Ultimate";
        ppState = new WillNeffComboStatePP(this);
        pState = new WillNeffComboStateP(this, ppState);
        kkState = new WillNeffComboStateKK(this);
        kState = new WillNeffComboStateK(this, kkState);
        defaultState = new WillNeffComboStateDefault(this, pState, kState);
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
    private WillNeffComboStateK kState;

    public WillNeffComboStateDefault(AttackManager attackManager, WillNeffComboStateP pState, 
        WillNeffComboStateK kState) : base(attackManager)
    {
        this.pState = pState;
        this.kState = kState;
    }

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Lunge Punch", 5, AttackType.Flinch);
        return pState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 7, AttackType.Flinch);
        return kState;
    }

    public override ComboState RangedAttack()
    {
        attackManager.QueueUpAttack("Shoot", 0, AttackType.Flinch);  //Possibly change logic for unique ranged attacks
        return endComboState;
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

        attackManager.QueueUpAttack("Lunge Punch", 1, AttackType.Flinch);
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
        attackManager.QueueUpAttack("Punch", 1, AttackType.KnockUp);
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

public class WillNeffComboStateK : ComboState
{
    private WillNeffComboStateKK kkState;

    public WillNeffComboStateK(AttackManager attackManager, WillNeffComboStateKK kkState) : base(attackManager)
    {
        this.kkState = kkState;
    }

    public override ComboState Punch()
    {
        return endComboState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 7, AttackType.Flinch);
        return kkState;
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

public class WillNeffComboStateKK : ComboState
{
    public WillNeffComboStateKK(AttackManager attackManager) : base(attackManager) { }

    public override ComboState Punch()
    {
        return endComboState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 7, AttackType.KnockBack);
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