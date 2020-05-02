using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackManager : MonoBehaviour
{
    protected Animator anim;
    private FighterController fighterController;
    private GameManager gameManager;
    private ComboState currentCombo;
    private EndComboState endComboState;
    protected bool readyForAttackInput;
    protected bool readyForAttackAnimation;
    private bool queuedAttack;
    private string queuedAttackAnimation;
    private int queuedAttackDamage;
    private AttackType queuedAttackType;
    private int queuedAttackSpecial;
    protected string cameraUltimateAnimation;

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        fighterController = GetComponent<FighterController>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
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

    public void CrouchPunch()
    {
        if (readyForAttackInput && currentCombo == GetDefaultState())
        {
            currentCombo = ((DefaultComboState)currentCombo).CrouchPunch();
        }
    }

    public void CrouchKick()
    {
        if (readyForAttackInput && currentCombo == GetDefaultState())
        {
            currentCombo = ((DefaultComboState)currentCombo).CrouchKick();
        }
    }

    public void CrouchRangedAttack()
    {
        if (readyForAttackInput && currentCombo == GetDefaultState())
        {
            currentCombo = ((DefaultComboState)currentCombo).CrouchRangedAttack();
        }
    }

    public void JumpPunch()
    {
        if (readyForAttackInput && currentCombo == GetDefaultState())
        {
            currentCombo = ((DefaultComboState)currentCombo).JumpPunch();
        }
    }

    public void JumpKick()
    {
        if (readyForAttackInput && currentCombo == GetDefaultState())
        {
            currentCombo = ((DefaultComboState)currentCombo).JumpKick();
        }
    }

    public void JumpRangedAttack()
    {
        if (readyForAttackInput && currentCombo == GetDefaultState())
        {
            currentCombo = ((DefaultComboState)currentCombo).JumpRangedAttack();
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

    public void PerformUltimateAttack(bool isPlayerOne)
    {
        anim.Play("Ultimate Attack");
        if (isPlayerOne)
        {
            gameManager.PlayZoomInCameraAnimation(cameraUltimateAnimation + " 1");
        }
        else
        {
            gameManager.PlayZoomInCameraAnimation(cameraUltimateAnimation + " 2");
        }
    }

    public void PerformComboAttackIfQueued()
    {
        if (readyForAttackAnimation && queuedAttack)
        {
            if (!queuedAttackAnimation.Contains("Crouch") && !queuedAttackAnimation.Contains("Jump"))
            {
                anim.Rebind();
            }
            anim.Play(queuedAttackAnimation);
            fighterController.SetAttackDamage(queuedAttackDamage);
            fighterController.SetAttackType(queuedAttackType);
            fighterController.ConsumeSpecial(queuedAttackSpecial);
            fighterController.StartAttack();
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

public abstract class DefaultComboState : ComboState
{
    public DefaultComboState(AttackManager attackManager) : base(attackManager) { }

    public ComboState CrouchPunch()
    {
        attackManager.QueueUpAttack("Crouch Punch", 3, AttackType.Flinch);
        return endComboState;
    }

    public ComboState CrouchKick()
    {
        attackManager.QueueUpAttack("Crouch Kick", 5, AttackType.KnockBack);
        return endComboState;
    }

    public ComboState CrouchRangedAttack()
    {
        attackManager.QueueUpAttack("Crouch Shoot", 0, AttackType.Flinch);
        return endComboState;
    }

    public ComboState JumpPunch()
    {
        attackManager.QueueUpAttack("Jump Punch", 3, AttackType.Flinch);
        return endComboState;
    }

    public ComboState JumpKick()
    {
        attackManager.QueueUpAttack("Jump Kick", 5, AttackType.KnockBack);
        return endComboState;
    }

    public ComboState JumpRangedAttack()
    {
        attackManager.QueueUpAttack("Jump Shoot", 0, AttackType.Flinch);
        return endComboState;
    }
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