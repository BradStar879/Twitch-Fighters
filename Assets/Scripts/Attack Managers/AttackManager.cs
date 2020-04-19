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
    private bool queuedComboAttack;
    private string queuedComboAttackAnimation;
    private int queuedComboAttackDamage;

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
        queuedComboAttack = false;
        queuedComboAttackAnimation = "";
        queuedComboAttackDamage = 0;
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

    public void RangeAttack()
    {
        if (readyForAttackInput)
        {
            currentCombo = currentCombo.RangeAttack();
            readyForAttackInput = false;
        }
    }

    public void SpecialAttack()
    {
        if (readyForAttackInput)
        {
            currentCombo = currentCombo.SpecialAttack();
            readyForAttackInput = false;
        }
    }

    public void Ultimate()
    {

    }

    public void PerformComboAttackIfQueued()
    {
        if (readyForAttackAnimation && queuedComboAttack)
        {
            anim.Rebind();
            anim.Play(queuedComboAttackAnimation);
            fighterController.SetAttackDamage(queuedComboAttackDamage);
            readyForAttackAnimation = false;
            queuedComboAttack = false;
            queuedComboAttackAnimation = "";
            queuedComboAttackDamage = 0;
        }
    }

    public void QueueUpAttack(string attackAnimation, int attackDamage)
    {
        queuedComboAttack = true;
        queuedComboAttackAnimation = attackAnimation;
        queuedComboAttackDamage = attackDamage;
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

    public abstract ComboState RangeAttack();

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

    public override ComboState RangeAttack()
    {
        return this;
    }

    public override ComboState SpecialAttack()
    {
        return this;
    }
}