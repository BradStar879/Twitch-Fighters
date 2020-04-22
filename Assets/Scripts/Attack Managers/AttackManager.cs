using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackManager : MonoBehaviour
{
    protected Animator anim;
    private FighterController fighterController;
    private ComboState currentCombo;
    private EndComboState endComboState;
    protected bool readyForAttackInput;
    protected bool readyForAttackAnimation;
    private bool queuedAttack;
    private string queuedAttackAnimation;
    private int queuedAttackDamage;
    private AttackType queuedAttackType;
    private int queuedAttackSpecial;

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        fighterController = GetComponent<FighterController>();
    }

    public void ResetCombo()
    {
        currentCombo = GetDefaultState();
        readyForAttackInput = true;
        readyForAttackAnimation = true;
        queuedAttack = false;
        queuedAttackAnimation = "";
        queuedAttackDamage = 0;
        queuedAttackType = AttackType.Flinch;
        queuedAttackSpecial = 0;
    }

    public void SetReadyForAttackInput(bool ready)
    {
        readyForAttackInput = ready;
    }

    public void SetReadyForAttackAnimation(bool ready)
    {
        readyForAttackAnimation = ready;
    }

    public void Punch()
    {
        if (readyForAttackInput)
        {
            currentCombo = currentCombo.Punch();
            readyForAttackInput = false;
        }
    }

    public void Kick()
    {
        if (readyForAttackInput)
        {
            currentCombo = currentCombo.Kick();
            readyForAttackInput = false;
        }
    }

    public void RangedAttack()
    {
        if (readyForAttackInput)
        {
            currentCombo = currentCombo.RangedAttack();
            readyForAttackInput = false;
        }
    }

    public void SpecialAttack()
    {
        if (readyForAttackInput)
        {
            currentCombo = currentCombo.SpecialAttack();
            if (currentCombo != GetDefaultState())  //Check for failed default special attack to prevent not being able to attack
            {
                readyForAttackInput = false;
            }
        }
    }

    public void Ultimate()
    {
        if (readyForAttackInput && fighterController.HasEnoughSpecial(50))  //Might need to change if special max changes
        {
            fighterController.StartUltimateAttack();
            anim.Play("Ultimate Setup");
        }
    }

    public void PerformComboAttackIfQueued()
    {
        if (readyForAttackAnimation && queuedAttack)
        {
            anim.Rebind();
            anim.Play(queuedAttackAnimation);
            fighterController.SetAttackDamage(queuedAttackDamage);
            fighterController.SetAttackType(queuedAttackType);
            fighterController.ConsumeSpecial(queuedAttackSpecial);
            readyForAttackAnimation = false;
            queuedAttack = false;
            queuedAttackAnimation = "";
            queuedAttackDamage = 0;
            queuedAttackType = AttackType.Flinch;
            queuedAttackSpecial = 0;
        }
    }

    public void QueueUpAttack(string attackAnimation, int attackDamage, AttackType attackType)
    {
        queuedAttack = true;
        queuedAttackAnimation = attackAnimation;
        queuedAttackDamage = attackDamage;
        queuedAttackType = attackType;
        queuedAttackSpecial = 0;
    }

    public void QueueUpAttack(string attackAnimation, int attackDamage, AttackType attackType, int attackSpecial)
    {
        queuedAttack = true;
        queuedAttackAnimation = attackAnimation;
        queuedAttackDamage = attackDamage;
        queuedAttackType = attackType;
        queuedAttackSpecial = attackSpecial;
    }

    public bool HasEnoughSpecial(int specialToUse)
    {
        return fighterController.HasEnoughSpecial(specialToUse);
    }

    public abstract ComboState GetDefaultState();
}

public abstract class ComboState
{
    protected AttackManager attackManager;
    protected ComboState endComboState;
    
    public ComboState(AttackManager attackManager)
    {
        this.attackManager = attackManager;
        if (!(GetType().Equals(typeof(EndComboState)))) {  //Stops infinite recursion in EndComboState constructor
            endComboState = new EndComboState(attackManager);
        }
    }

    public abstract ComboState Punch();

    public abstract ComboState Kick();

    public abstract ComboState RangedAttack();

    public abstract ComboState SpecialAttack();
}

public class EndComboState : ComboState
{
    public EndComboState(AttackManager attackManager) : base(attackManager) { }

    public override ComboState Punch()
    {
        return this;
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
        return this;
    }
}

public enum AttackType
{
    Flinch,
    KnockBack,
    KnockUp
}