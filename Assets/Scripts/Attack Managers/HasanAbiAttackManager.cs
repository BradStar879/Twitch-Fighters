using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasanAbiAttackManager : AttackManager
{
    private HasanAbiComboStateDefault defaultState;
    private HasanAbiComboStateP pState;
    private HasanAbiComboStatePP ppState;
    private HasanAbiComboStatePPR pprState;
    private HasanAbiComboStatePR prState;
    private HasanAbiComboStatePRK prkState;
    private HasanAbiComboStatePRKR prkrState;
    private HasanAbiComboStateK kState;
    private HasanAbiComboStateKK kkState;

    public override void Init()
    {
        base.Init();
        cameraUltimateAnimation = "HasanAbi Ultimate";
        prkrState = new HasanAbiComboStatePRKR(this);
        prkState = new HasanAbiComboStatePRK(this, prkrState);
        prState = new HasanAbiComboStatePR(this, prkState);
        pprState = new HasanAbiComboStatePPR(this);
        ppState = new HasanAbiComboStatePP(this, pprState);
        pState = new HasanAbiComboStateP(this, ppState);
        kkState = new HasanAbiComboStateKK(this);
        kState = new HasanAbiComboStateK(this, kkState);
        defaultState = new HasanAbiComboStateDefault(this, pState, kState);
        ResetCombo();
    }

    public override ComboState GetDefaultState()
    {
        return defaultState;
    }
}

public class HasanAbiComboStateDefault : DefaultComboState
{
    private HasanAbiComboStateP pState;
    private HasanAbiComboStateK kState;

    public HasanAbiComboStateDefault(AttackManager attackManager, HasanAbiComboStateP pState,
        HasanAbiComboStateK kState) : base(attackManager)
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

public class HasanAbiComboStateP : ComboState
{
    private HasanAbiComboStatePP ppState;

    public HasanAbiComboStateP(AttackManager attackManager, HasanAbiComboStatePP ppState) : base(attackManager)
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
        attackManager.QueueUpAttack("Shoot", 0, AttackType.Flinch);
        return ppState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}

public class HasanAbiComboStatePP : ComboState
{
    private HasanAbiComboStatePPR pprState;

    public HasanAbiComboStatePP(AttackManager attackManager, HasanAbiComboStatePPR pprState) : base(attackManager)
    {
        this.pprState = pprState;
    }

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Punch", 1, AttackType.KnockBack);
        return endComboState;
    }

    public override ComboState Kick()
    {
        return endComboState;
    }

    public override ComboState RangedAttack()
    {
        attackManager.QueueUpAttack("Shoot", 0, AttackType.Flinch);
        return pprState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}

public class HasanAbiComboStatePPR : ComboState
{
    public HasanAbiComboStatePPR(AttackManager attackManager) : base(attackManager) { }

    public override ComboState Punch()
    {
        return endComboState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 4, AttackType.KnockUp);
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

public class HasanAbiComboStatePR : ComboState
{
    private HasanAbiComboStatePRK prkState;

    public HasanAbiComboStatePR(AttackManager attackManager, HasanAbiComboStatePRK prkState) : base(attackManager)
    {
        this.prkState = prkState;
    }

    public override ComboState Punch()
    {
        return endComboState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 4, AttackType.Flinch);
        return prkState;
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

public class HasanAbiComboStatePRK : ComboState
{
    private HasanAbiComboStatePRKR prkrState;

    public HasanAbiComboStatePRK(AttackManager attackManager, HasanAbiComboStatePRKR prkrState) : base(attackManager)
    {
        this.prkrState = prkrState;
    }

    public override ComboState Punch()
    {
        return endComboState;
    }

    public override ComboState Kick()
    {
        return endComboState;
    }

    public override ComboState RangedAttack()
    {
        attackManager.QueueUpAttack("Shoot", 0, AttackType.Flinch);
        return prkrState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}

public class HasanAbiComboStatePRKR : ComboState
{
    public HasanAbiComboStatePRKR(AttackManager attackManager) : base(attackManager) { }

    public override ComboState Punch()
    {
        return endComboState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 3, AttackType.KnockUp);
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

public class HasanAbiComboStateK : ComboState
{
    private HasanAbiComboStateKK kkState;

    public HasanAbiComboStateK(AttackManager attackManager, HasanAbiComboStateKK kkState) : base(attackManager)
    {
        this.kkState = kkState;
    }

    public override ComboState Punch()
    {
        return endComboState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 3, AttackType.KnockUp);
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

public class HasanAbiComboStateKK : ComboState
{
    public HasanAbiComboStateKK(AttackManager attackManager) : base(attackManager) { }

    public override ComboState Punch()
    {
        return endComboState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 5, AttackType.Flinch);
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
