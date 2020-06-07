using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NesuaAttackManager : AttackManager
{
    private NesuaComboStateDefault defaultState;
    private NesuaComboStateP pState;
    private NesuaComboStatePP ppState;
    private NesuaComboStatePPR pprState;
    private NesuaComboStatePR prState;
    private NesuaComboStatePRK prkState;
    private NesuaComboStatePRKR prkrState;
    private NesuaComboStateK kState;
    private NesuaComboStateKK kkState;

    public override void Init()
    {
        base.Init();
        cameraUltimateAnimation = "Nesua Ultimate";
        prkrState = new NesuaComboStatePRKR(this);
        prkState = new NesuaComboStatePRK(this, prkrState);
        prState = new NesuaComboStatePR(this, prkState);
        pprState = new NesuaComboStatePPR(this);
        ppState = new NesuaComboStatePP(this, pprState);
        pState = new NesuaComboStateP(this, ppState);
        kkState = new NesuaComboStateKK(this);
        kState = new NesuaComboStateK(this, kkState);
        defaultState = new NesuaComboStateDefault(this, pState, kState);
        ResetCombo();
    }

    public override ComboState GetDefaultState()
    {
        return defaultState;
    }
}

public class NesuaComboStateDefault : DefaultComboState
{
    private NesuaComboStateP pState;
    private NesuaComboStateK kState;

    public NesuaComboStateDefault(AttackManager attackManager, NesuaComboStateP pState,
        NesuaComboStateK kState) : base(attackManager)
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

public class NesuaComboStateP : ComboState
{
    private NesuaComboStatePP ppState;

    public NesuaComboStateP(AttackManager attackManager, NesuaComboStatePP ppState) : base(attackManager)
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

public class NesuaComboStatePP : ComboState
{
    private NesuaComboStatePPR pprState;

    public NesuaComboStatePP(AttackManager attackManager, NesuaComboStatePPR pprState) : base(attackManager)
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

public class NesuaComboStatePPR : ComboState
{
    public NesuaComboStatePPR(AttackManager attackManager) : base(attackManager) { }

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

public class NesuaComboStatePR : ComboState
{
    private NesuaComboStatePRK prkState;

    public NesuaComboStatePR(AttackManager attackManager, NesuaComboStatePRK prkState) : base(attackManager)
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

public class NesuaComboStatePRK : ComboState
{
    private NesuaComboStatePRKR prkrState;

    public NesuaComboStatePRK(AttackManager attackManager, NesuaComboStatePRKR prkrState) : base(attackManager)
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

public class NesuaComboStatePRKR : ComboState
{
    public NesuaComboStatePRKR(AttackManager attackManager) : base(attackManager) { }

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

public class NesuaComboStateK : ComboState
{
    private NesuaComboStateKK kkState;

    public NesuaComboStateK(AttackManager attackManager, NesuaComboStateKK kkState) : base(attackManager)
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

public class NesuaComboStateKK : ComboState
{
    public NesuaComboStateKK(AttackManager attackManager) : base(attackManager) { }

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
