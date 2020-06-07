using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class merely exists as a template for creating new fighters
public class DefaultFighterAttackManager : AttackManager
{
    private DefaultComboStateDefault defaultState;
    private DefaultComboStateP pState;
    private DefaultComboStatePP ppState;
    private DefaultComboStatePPR pprState;
    private DefaultComboStatePR prState;
    private DefaultComboStatePRK prkState;
    private DefaultComboStatePRKR prkrState;
    private DefaultComboStateK kState;
    private DefaultComboStateKK kkState;

    public override void Init()
    {
        base.Init();
        cameraUltimateAnimation = "Default Ultimate";
        prkrState = new DefaultComboStatePRKR(this);
        prkState = new DefaultComboStatePRK(this, prkrState);
        prState = new DefaultComboStatePR(this, prkState);
        pprState = new DefaultComboStatePPR(this);
        ppState = new DefaultComboStatePP(this, pprState);
        pState = new DefaultComboStateP(this, ppState);
        kkState = new DefaultComboStateKK(this);
        kState = new DefaultComboStateK(this, kkState);
        defaultState = new DefaultComboStateDefault(this, pState, kState);
        ResetCombo();
    }

    public override ComboState GetDefaultState()
    {
        return defaultState;
    }
}

public class DefaultComboStateDefault : DefaultComboState
{
    private DefaultComboStateP pState;
    private DefaultComboStateK kState;

    public DefaultComboStateDefault(AttackManager attackManager, DefaultComboStateP pState,
        DefaultComboStateK kState) : base(attackManager)
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

public class DefaultComboStateP : ComboState
{
    private DefaultComboStatePP ppState;

    public DefaultComboStateP(AttackManager attackManager, DefaultComboStatePP ppState) : base(attackManager)
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

public class DefaultComboStatePP : ComboState
{
    private DefaultComboStatePPR pprState;

    public DefaultComboStatePP(AttackManager attackManager, DefaultComboStatePPR pprState) : base(attackManager)
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

public class DefaultComboStatePPR : ComboState
{
    public DefaultComboStatePPR(AttackManager attackManager) : base(attackManager) { }

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

public class DefaultComboStatePR : ComboState
{
    private DefaultComboStatePRK prkState;

    public DefaultComboStatePR(AttackManager attackManager, DefaultComboStatePRK prkState) : base(attackManager)
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

public class DefaultComboStatePRK : ComboState
{
    private DefaultComboStatePRKR prkrState;

    public DefaultComboStatePRK(AttackManager attackManager, DefaultComboStatePRKR prkrState) : base(attackManager)
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

public class DefaultComboStatePRKR : ComboState
{
    public DefaultComboStatePRKR(AttackManager attackManager) : base(attackManager) { }

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

public class DefaultComboStateK : ComboState
{
    private DefaultComboStateKK kkState;

    public DefaultComboStateK(AttackManager attackManager, DefaultComboStateKK kkState) : base(attackManager)
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

public class DefaultComboStateKK : ComboState
{
    public DefaultComboStateKK(AttackManager attackManager) : base(attackManager) { }

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
