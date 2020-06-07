using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillNeffAttackManager : AttackManager
{
    private WillNeffComboStateDefault defaultState;
    private WillNeffComboStateP pState;
    private WillNeffComboStatePP ppState;
    private WillNeffComboStatePPR pprState;
    private WillNeffComboStatePR prState;
    private WillNeffComboStatePRK prkState;
    private WillNeffComboStatePRKR prkrState;
    private WillNeffComboStatePRR prrState;
    private WillNeffComboStatePRRK prrkState;
    private WillNeffComboStatePRRKR prrkrState;
    private WillNeffComboStatePK pkState;
    private WillNeffComboStatePKK pkkState;
    private WillNeffComboStatePKKR pkkrState;
    private WillNeffComboStatePKR pkrState;
    private WillNeffComboStateK kState;
    private WillNeffComboStateKK kkState;

    public override void Init()
    {
        base.Init();
        cameraUltimateAnimation = "Will Neff Ultimate";
        pkrState = new WillNeffComboStatePKR(this);
        pkkrState = new WillNeffComboStatePKKR(this);
        pkkState = new WillNeffComboStatePKK(this, pkkrState);
        pkState = new WillNeffComboStatePK(this, pkkState, pkrState);
        prrkrState = new WillNeffComboStatePRRKR(this);
        prrkState = new WillNeffComboStatePRRK(this, prrkrState);
        prrState = new WillNeffComboStatePRR(this, prrkState);
        prkrState = new WillNeffComboStatePRKR(this);
        prkState = new WillNeffComboStatePRK(this, prkrState);
        prState = new WillNeffComboStatePR(this, prkState, prrState);
        pprState = new WillNeffComboStatePPR(this);
        ppState = new WillNeffComboStatePP(this, pprState);
        pState = new WillNeffComboStateP(this, ppState, prState, pkState);
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

public class WillNeffComboStateDefault : DefaultComboState
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
    private WillNeffComboStatePR prState;
    private WillNeffComboStatePK pkState;

    public WillNeffComboStateP(AttackManager attackManager, WillNeffComboStatePP ppState,
        WillNeffComboStatePR prState, WillNeffComboStatePK pkState) : base(attackManager)
    {
        this.ppState = ppState;
        this.prState = prState;
        this.pkState = pkState;
    }

    public override ComboState Punch()
    {

        attackManager.QueueUpAttack("Lunge Punch", 2, AttackType.Flinch);
        return ppState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 3, AttackType.Flinch);
        return pkState;
    }

    public override ComboState RangedAttack()
    {
        attackManager.QueueUpAttack("Shoot", 0, AttackType.Flinch);
        return prState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}

public class WillNeffComboStatePP : ComboState
{
    private WillNeffComboStatePPR pprState;

    public WillNeffComboStatePP(AttackManager attackManager, WillNeffComboStatePPR pprState) : base(attackManager) 
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

public class WillNeffComboStatePPR : ComboState
{
    public WillNeffComboStatePPR(AttackManager attackManager) : base(attackManager) { }

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

public class WillNeffComboStatePR : ComboState
{
    private WillNeffComboStatePRK prkState;
    private WillNeffComboStatePRR prrState;

    public WillNeffComboStatePR(AttackManager attackManager, WillNeffComboStatePRK prkState,
        WillNeffComboStatePRR prrState) : base(attackManager)
    {
        this.prkState = prkState;
        this.prrState = prrState;
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
        attackManager.QueueUpAttack("Shoot", 0, AttackType.Flinch);
        return prrState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}

public class WillNeffComboStatePRK : ComboState
{
    private WillNeffComboStatePRKR prkrState;

    public WillNeffComboStatePRK(AttackManager attackManager, WillNeffComboStatePRKR prkrState) : base(attackManager)
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

public class WillNeffComboStatePRKR : ComboState
{
    public WillNeffComboStatePRKR(AttackManager attackManager) : base(attackManager) {}

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

public class WillNeffComboStatePRR : ComboState
{
    private WillNeffComboStatePRRK prrkState;

    public WillNeffComboStatePRR(AttackManager attackManager, WillNeffComboStatePRRK prrkState) : base(attackManager)
    {
        this.prrkState = prrkState;
    }

    public override ComboState Punch()
    {
        return endComboState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 5, AttackType.KnockUp);
        return prrkState;
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

public class WillNeffComboStatePRRK : ComboState
{
    private WillNeffComboStatePRRKR prrkrState;

    public WillNeffComboStatePRRK(AttackManager attackManager, WillNeffComboStatePRRKR prrkrState) : base(attackManager)
    {
        this.prrkrState = prrkrState;
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
        return prrkrState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}

public class WillNeffComboStatePRRKR : ComboState
{
    public WillNeffComboStatePRRKR(AttackManager attackManager) : base(attackManager) { }

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Punch", 5, AttackType.Flinch);
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
            attackManager.QueueUpAttack("Punch", 11, AttackType.Flinch, 10);
        }
        return endComboState;
    }
}

public class WillNeffComboStatePK : ComboState
{
    private WillNeffComboStatePKK pkkState;
    private WillNeffComboStatePKR pkrState;

    public WillNeffComboStatePK(AttackManager attackManager, WillNeffComboStatePKK pkkState,
        WillNeffComboStatePKR pkrState) : base(attackManager) 
    {
        this.pkkState = pkkState;
        this.pkrState = pkrState;
    }

    public override ComboState Punch()
    {
        return endComboState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 5, AttackType.Flinch);
        return pkkState;
    }

    public override ComboState RangedAttack()
    {
        attackManager.QueueUpAttack("Shoot", 0, AttackType.Flinch);
        return pkrState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}

public class WillNeffComboStatePKK : ComboState
{
    private WillNeffComboStatePKKR pkkrState;

    public WillNeffComboStatePKK(AttackManager attackManager, WillNeffComboStatePKKR pkkrState) : base(attackManager)
    {
        this.pkkrState = pkkrState;
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
        return pkkrState;
    }

    public override ComboState SpecialAttack()
    {
        return endComboState;
    }
}

public class WillNeffComboStatePKKR : ComboState
{
    public WillNeffComboStatePKKR(AttackManager attackManager) : base(attackManager) {}

    public override ComboState Punch()
    {
        attackManager.QueueUpAttack("Punch", 5, AttackType.KnockBack);
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
            attackManager.QueueUpAttack("Punch", 5, AttackType.KnockUp, 10);
        }
        return endComboState;
    }
}

public class WillNeffComboStatePKR : ComboState
{
    public WillNeffComboStatePKR(AttackManager attackManager) : base(attackManager) { }

    public override ComboState Punch()
    {
        return endComboState;
    }

    public override ComboState Kick()
    {
        attackManager.QueueUpAttack("Kick", 5, AttackType.KnockUp);
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
            attackManager.QueueUpAttack("Kick", 5, AttackType.KnockUp, 10);
        }
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

public class WillNeffComboStateKK : ComboState
{
    public WillNeffComboStateKK(AttackManager attackManager) : base(attackManager) { }

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