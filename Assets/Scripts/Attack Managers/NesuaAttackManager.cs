using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NesuaAttackManager : AttackManager
{
    private NesuaComboStateDefault defaultState;
    private NesuaComboStateP pState;
    private NesuaComboStatePP ppState;
    private NesuaComboStatePPR pprState;
    private NesuaComboStatePPRR pprrState;
    private NesuaComboStatePR prState;
    private NesuaComboStatePRK prkState;
    private NesuaComboStatePRKR prkrState;
    private NesuaComboStatePRKP prkpState;
    private NesuaComboStateK kState;
    private NesuaComboStateKK kkState;
    private NesuaComboStateKP kpState;
    private NesuaComboStateKPP kppState;

    public override void Init()
    {
        base.Init();
        cameraUltimateAnimation = "Nesua Ultimate";
        prkpState = new NesuaComboStatePRKP(this);
        prkrState = new NesuaComboStatePRKR(this);
        prkState = new NesuaComboStatePRK(this, prkrState, prkpState);
        prState = new NesuaComboStatePR(this, prkState);
        pprrState = new NesuaComboStatePPRR(this);
        pprState = new NesuaComboStatePPR(this, pprrState);
        ppState = new NesuaComboStatePP(this, pprState);
        pState = new NesuaComboStateP(this, ppState);
        kppState = new NesuaComboStateKPP(this);
        kpState = new NesuaComboStateKP(this, kppState);
        kkState = new NesuaComboStateKK(this);
        kState = new NesuaComboStateK(this, kkState, kpState);
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
    private NesuaComboStatePPRR pprrState;

    public NesuaComboStatePPR(AttackManager attackManager, NesuaComboStatePPRR pprrState) : base(attackManager) 
    {
        this.pprrState = pprrState;
    }

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
        attackManager.QueueUpAttack("Shoot", 5, AttackType.Flinch);
        return pprrState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}

public class NesuaComboStatePPRR : ComboState
{
    public NesuaComboStatePPRR(AttackManager attackManager) : base(attackManager) { }

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Punch", 6, AttackType.KnockBack);
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
        if (attackManager.HasEnoughSpecial(10)) 
        {
            attackManager.QueueUpAttack("Punch", 10, AttackType.KnockBack, 10);
        }
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
    private NesuaComboStatePRKP prkpState;

    public NesuaComboStatePRK(AttackManager attackManager, NesuaComboStatePRKR prkrState, 
        NesuaComboStatePRKP prkpState) : base(attackManager)
    {
        this.prkrState = prkrState;
        this.prkpState = prkpState;
    }

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Punch", 4, AttackType.KnockUp);
        return prkpState;
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

public class NesuaComboStatePRKP : ComboState
{
    public NesuaComboStatePRKP(AttackManager attackManager) : base(attackManager) { }

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Punch", 6, AttackType.Flinch);
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
        if (attackManager.HasEnoughSpecial(10))
        {
            attackManager.QueueUpAttack("Punch", 10, AttackType.KnockBack, 10);
        }
        return endComboState;
    }
}

public class NesuaComboStateK : ComboState
{
    private NesuaComboStateKK kkState;
    private NesuaComboStateKP kpState;

    public NesuaComboStateK(AttackManager attackManager, NesuaComboStateKK kkState, NesuaComboStateKP kpState) : base(attackManager)
    {
        this.kkState = kkState;
        this.kpState = kpState;
    }

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Punch", 3, AttackType.Flinch);
        return kpState;
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

public class NesuaComboStateKP : ComboState
{
    private NesuaComboStateKPP kppState;

    public NesuaComboStateKP(AttackManager attackManager, NesuaComboStateKPP kppState) : base(attackManager)
    {
        this.kppState = kppState;
    }

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Punch", 4, AttackType.Flinch);
        return kppState;
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

public class NesuaComboStateKPP : ComboState
{
    public NesuaComboStateKPP(AttackManager attackManager) : base(attackManager) { }

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Punch", 6, AttackType.KnockBack);
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
        if (attackManager.HasEnoughSpecial(10))
        {
            attackManager.QueueUpAttack("Punch", 6, AttackType.KnockUp, 10);
        }
        return endComboState;
    }
}