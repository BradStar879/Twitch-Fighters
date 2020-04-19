using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackManager : MonoBehaviour
{
    protected Animator anim;
    private ComboState currentCombo;
    private EndComboState endComboState;
    protected bool readyForComboAttack;

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        endComboState = new EndComboState(anim);
    }

    public void ResetCombo()
    {
        currentCombo = GetDefaultState();
        readyForComboAttack = false;
    }

    public void SetReadyForComboAttack(bool ready)
    {
        readyForComboAttack = ready;
    }

    public void Punch()
    {
        if (InDefaultState() ||
            (!InDefaultState() && readyForComboAttack))
        {
            currentCombo = currentCombo.Punch();
            readyForComboAttack = false;
        }
        /*else
        {
            currentCombo = endComboState;
        }*/
        
    }

    public void Kick()
    {
        if (InDefaultState() ||
            (!InDefaultState() && readyForComboAttack))
        {
            currentCombo = currentCombo.Kick();
            readyForComboAttack = false;
        }
        /*else
        {
            currentCombo = endComboState;
        }*/
    }

    public void RangeAttack()
    {
        if (InDefaultState() ||
            (!InDefaultState() && readyForComboAttack))
        {
            currentCombo = currentCombo.RangeAttack();
            readyForComboAttack = false;
        }
        /*else
        {
            currentCombo = endComboState;
        }*/
    }

    public void SpecialAttack()
    {
        if (InDefaultState() ||
            (!InDefaultState() && readyForComboAttack))
        {
            currentCombo = currentCombo.SpecialAttack();
            readyForComboAttack = false;
        }
        /*else
        {
            currentCombo = endComboState;
        }*/
    }

    public void Ultimate()
    {

    }

    private bool InDefaultState()
    {
        //print("current combo: " + currentCombo);
        //print("default state" + GetDefaultState());
        return currentCombo == GetDefaultState();
    }

    public abstract ComboState GetDefaultState();
}

public abstract class ComboState
{
    protected Animator anim;
    protected ComboState endComboState;
    
    public ComboState(Animator anim)
    {
        this.anim = anim;
        if (!(GetType().Equals(typeof(EndComboState)))) {  //Stops infinite recursion in EndComboState constructor
            endComboState = new EndComboState(anim);
        }
    }

    public abstract ComboState Punch();

    public abstract ComboState Kick();

    public abstract ComboState RangeAttack();

    public abstract ComboState SpecialAttack();
}

public class EndComboState : ComboState
{
    public EndComboState(Animator anim) : base(anim) { }

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