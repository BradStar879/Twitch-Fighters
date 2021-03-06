﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    ControllerInput controllerInput;
    private bool isPlayerOne = true;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] GameObject projectile;
    private AttackManager attackManager;
    private FighterControllerState fighterControllerState;
    private GameObject otherFighter;
    private FighterController otherFighterController;
    private GameManager gameManager;
    private Rigidbody rb;
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 startingScale;
    private int hp; //Max is 100
    private int special; //Max is 50
    private FighterUI fighterUI;
    private Stance stance;
    private Action action;
    [SerializeField] Collider leftShinCollider;
    [SerializeField] Collider leftFootCollider;
    [SerializeField] Collider leftThighCollider;
    [SerializeField] Collider rightShinCollider;
    [SerializeField] Collider rightFootCollider;
    [SerializeField] Collider rightThighCollider;
    [SerializeField] Collider rightHandCollider;
    [SerializeField] Collider rightForearmCollider;
    [SerializeField] ParticleSystem sparkParticles;
    private Animator anim;
    private bool delayFrame;
    private bool collidingWithEnemy;
    private bool ultimateAttacking;
    private bool attackMoving;
    private bool inHitFrame;
    private bool invincible;
    private Vector3 attackingVelocity;
    private int currentAttackDamage;
    private AttackType currentAttackType;

    private Material fighterMaterial;

    //To be populated via Unity
    [SerializeField] string[] introQuotes;
    [SerializeField] string[] victoryQuotes;

    public virtual void Init(bool isPlayerOne, GameObject otherFighter)
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        this.isPlayerOne = isPlayerOne;
        this.otherFighter = otherFighter;
        otherFighterController = otherFighter.GetComponent<FighterController>();
        fighterControllerState = FighterControllerState.DetermineFighterControllerState(this);

        if (isPlayerOne)
        {
            controllerInput = ControllerManager.GetControllerInput(1);
            fighterUI = canvas.transform.GetChild(0).GetComponent<FighterUI>();
        }
        else
        {
            controllerInput = ControllerManager.GetControllerInput(2);
            fighterUI = canvas.transform.GetChild(1).GetComponent<FighterUI>();
        }
        anim = GetComponent<Animator>();
        attackManager = GetComponent<AttackManager>();
        attackManager.Init();
        fighterUI.Init();

        startingPosition.Set(transform.position.x, transform.position.y, transform.position.z);
        startingRotation.Set(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        startingScale.Set(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGameActive()) {
            fighterControllerState.FighterUpdate();
        }
        if (delayFrame)
        {
            delayFrame = false;
        }
    }

    public void PlayerUpdate()
    {
        if (controllerInput.GetStartButtonDown())
        {
            gameManager.PauseGame(isPlayerOne);
        }

        if (action == Action.Neutral || action == Action.Attacking) //Attacks managed here
        {
            if (controllerInput.GetLeftActionButtonDown())
            {
                attackManager.Punch(stance);
            }
            else if (controllerInput.GetBottomActionButtonDown() && !delayFrame)    //Same button as resuming from pause menu so wait a frame
            {
                attackManager.Kick(stance);
            }
            else if (controllerInput.GetTopActionButtonDown())
            {
                attackManager.RangedAttack(stance);
            }

            if (stance == Stance.Standing)
            {
                if (controllerInput.GetLeftBumperDown())
                {
                    attackManager.Taunt();
                }
                else if (!controllerInput.GetRightBumper() && controllerInput.GetRightActionButtonDown())
                {
                    attackManager.SpecialAttack();
                }
                else if (controllerInput.GetRightBumper() && controllerInput.GetRightActionButtonDown())
                {
                    attackManager.Ultimate();
                }
            }

            attackManager.PerformComboAttackIfQueued();
        }

        if (action != Action.Recovering && action != Action.Attacking && stance != Stance.KnockedDown)
        {
            Vector3 velocity = new Vector3(0f, rb.velocity.y);
            if (stance == Stance.Jumping)
            {
                velocity.x = rb.velocity.x;
            }
            else if (stance == Stance.Standing || stance == Stance.Crouching) //Blocking, moving, and crouching inputs
            {
                if (controllerInput.GetXAxisLeft()) //Movement
                {
                    velocity.x -= moveSpeed;
                    anim.SetFloat("direction", -1f);
                    if (!anim.GetBool("walking"))
                    {
                        anim.Play("Walk", 0, float.NegativeInfinity);
                    }
                }
                else if (controllerInput.GetXAxisRight())
                {
                    velocity.x += moveSpeed;
                    anim.SetFloat("direction", 1f);
                    if (!anim.GetBool("walking"))
                    {
                        anim.Play("Walk");
                    }
                }

                if (action == Action.Blocking)
                {
                    if (!(controllerInput.GetLeftTrigger() || controllerInput.GetRightTrigger()))
                    {
                        anim.Play("Unblock");
                    }
                }
                else
                {
                    if (controllerInput.GetLeftTrigger() || controllerInput.GetRightTrigger())
                    {
                        anim.Play("Block");
                        action = Action.Recovering;
                    }
                }

                if (action != Action.Attacking)
                {
                    if (stance == Stance.Crouching)
                    {
                        if (!controllerInput.GetYAxisDown())
                        {
                            anim.Play("Uncrouch");
                        }
                    }
                    else
                    {
                        if (controllerInput.GetYAxisDown())
                        {
                            anim.Play("Crouch");
                        }
                    }
                }
            }

            if (stance == Stance.Standing && action == Action.Neutral)
            {
                if (controllerInput.GetYAxisUp())
                {
                    invincible = false;
                    stance = Stance.Jumping;
                    velocity.y = 2f;
                    //Add jumping animation
                }
            }

            if (!gameManager.AbleToMoveForward() && //Prevent player from moving past other player
                ((isPlayerOne && velocity.x > 0f) ||
                (!isPlayerOne && velocity.x < 0f)))
            {
                velocity.x = 0f;

            }
            rb.velocity = velocity;
            if (velocity.x == 0f)
            {
                anim.SetBool("walking", false);
            }
            else
            {
                anim.SetBool("walking", true);
            }
        }
        else if (attackMoving)
        {
            rb.velocity = attackingVelocity;
        }
        else if (stance == Stance.KnockedDown && action != Action.Recovering)
        {
            if (controllerInput.GetYAxisUp())  
            {
                GetUp();
            }
        }

        if (inHitFrame)
        {
            CheckForHit();
        }

    }

    public void EasyAiUpdate()
    {
        if (stance == Stance.KnockedDown && action != Action.Recovering)
        {
            GetUp();
        }

        if (inHitFrame)
        {
            CheckForHit();
        }
    }

    public void MediumAiUpdate()
    {
        if (stance == Stance.KnockedDown && action != Action.Recovering)
        {
            GetUp();
        }

        if (inHitFrame)
        {
            CheckForHit();
        }
    }

    public void HardAiUpdate()
    {
        if (stance == Stance.KnockedDown && action != Action.Recovering)
        {
            GetUp();
        }

        if (inHitFrame)
        {
            CheckForHit();
        }
    }

    /*private void FadeIn(Transform t)
    {
        t.GetComponent<Material>().SetFloat("_DissolveAmount", .5f);
        for (int i = 0; i < t.childCount; i++)
        {
            FadeIn(t.GetChild(i));
        }
    }*/

    public FighterUI GetFighterUI()
    {
        return fighterUI;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    public Animator GetAnimator()
    {
        return anim;
    }

    public bool IsPlayerOne()
    {
        return isPlayerOne;
    }

    public void ResetFighter()
    {
        ChangeFighterVisibility(true);
        anim.Rebind();
        anim.Play("Idle");
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        transform.localScale = startingScale;
        hp = 100;
        special = 0;
        attackingVelocity = Vector3.zero;
        fighterUI.UpdateHp(hp);
        fighterUI.UpdateSpecial(special);
        stance = Stance.Standing;
        action = Action.Neutral;
        delayFrame = false;
        collidingWithEnemy = false;
        inHitFrame = false;
        ultimateAttacking = false;
        invincible = false;
        TurnLegCollidersIntoColliders();
        rightHandCollider.isTrigger = false;
        rightForearmCollider.isTrigger = false;
    }

    public void ChangeFighterVisibility(bool visible)
    {
        foreach (Renderer childRenderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            childRenderer.enabled = visible;
        }
    }

    public void TakeDamage(int damage, AttackType attackType)
    {
        EndAttack();
        action = Action.Recovering;
        inHitFrame = false;
        attackMoving = false;
        anim.Rebind();  //Stops playback on all layers

        hp -= damage;
        if (hp > 0)
        {
            if (stance == Stance.KnockedUp)
            {
                rb.velocity = new Vector3(rb.velocity.x, 3f, rb.velocity.z);
                anim.Play("Juggle");
            }
            else if (stance == Stance.KnockedDown)
            {
                anim.Play("Flop");
                invincible = true;
            }
            else
            {
                if (attackType == AttackType.Flinch)
                {
                    anim.Play("Flinch");
                }
                else if (attackType == AttackType.KnockBack)
                {
                    invincible = true;
                    stance = Stance.KnockedDown;
                    float velX = -2f;
                    if (!isPlayerOne)
                    {
                        velX = 2f;
                    }
                    rb.velocity = new Vector3(velX, 2f, rb.velocity.z);
                    anim.Play("Knock Back");

                }
                else if (attackType == AttackType.KnockUp)
                {
                    float velX = -.4f;
                    if (!isPlayerOne)
                    {
                        velX = .4f;
                    }
                    rb.velocity = new Vector3(velX, 3.5f, rb.velocity.z);
                    stance = Stance.KnockedUp;
                    TurnLegCollidersIntoTriggers();
                    anim.Play("Knock Up");
                }
            }
            ChargeSpecial(damage / 3);
        }
        else
        {
            anim.Play("Knock Back");
            hp = 0;
            gameManager.EndRound(!isPlayerOne);
        } 
        fighterUI.UpdateHp(hp);
    }

    public void Recover()
    {
        action = Action.Neutral;
    }

    public void SetAttackDamage(int attackDamage)
    {
        currentAttackDamage = attackDamage;
    }

    public void SetAttackType(AttackType attackType)
    {
        currentAttackType = attackType;
    }

    private void DisableInvincibility()
    {
        invincible = false;
    }

    public void EndHitFrame()
    {
        inHitFrame = false;
    }

    private void CheckForHit()
    {
        if (collidingWithEnemy)
        {
            if (!ultimateAttacking)
            {
                AttackEnemy();
            }
            else
            {
                UltimateAttackEnemy();
            }
            inHitFrame = false;
        }
    }

    public void StartKickHitFrame()
    {
        inHitFrame = true;
        rightShinCollider.isTrigger = true;
        rightFootCollider.isTrigger = true;
    }

    private void StartPunchHitFrame()
    {
        inHitFrame = true;
        rightHandCollider.isTrigger = true;
        rightForearmCollider.isTrigger = true;
    }

    private void ShootProjectile()
    {
        Vector3 projectilePosition = new Vector3(transform.position.x + .5f, .94f, transform.position.z);
        if (!isPlayerOne)
        {
            projectilePosition.x = transform.position.x - .5f;
        }
        GameObject projectileClone = Instantiate(projectile, projectilePosition, Quaternion.identity);
        projectileClone.SetActive(true);
        projectileClone.GetComponent<ProjectileScript>().Init(isPlayerOne);
    }

    private void ShootProjectileLow()
    {
        Vector3 projectilePosition = new Vector3(transform.position.x + .5f, .82f, transform.position.z);
        if (!isPlayerOne)
        {
            projectilePosition.x = transform.position.x - .5f;
        }
        GameObject projectileClone = Instantiate(projectile, projectilePosition, Quaternion.identity);
        projectileClone.SetActive(true);
        projectileClone.GetComponent<ProjectileScript>().Init(isPlayerOne);
    }

    private void ShootProjectileHigh()
    {
        Vector3 projectilePosition = new Vector3(transform.position.x + .5f, transform.position.y + .2f, transform.position.z);
        if (!isPlayerOne)
        {
            projectilePosition.x = transform.position.x - .5f;
        }
        GameObject projectileClone = Instantiate(projectile, projectilePosition, Quaternion.identity);
        projectileClone.SetActive(true);
        projectileClone.GetComponent<ProjectileScript>().Init(isPlayerOne);
        projectileClone.GetComponent<ProjectileScript>().SetDownwardVelocity();
    }

    public void ConsumeSpecial(int specialToUse)
    {
        special -= specialToUse;
        fighterUI.UpdateSpecial(special);
    }

    public bool HasEnoughSpecial(int specialToUse)
    {
        return specialToUse <= special;
    }

    private void ChargeSpecialFromTaunt()
    {
        ChargeSpecial(10);
        EndAttack();
    }

    public void ChargeSpecial(int amount)
    {
        special += amount;
        if (special > 50)
        {
            special = 50;
        }
        fighterUI.UpdateSpecial(special);
    }

    public void StartUltimateAttack()
    {
        ConsumeSpecial(50);
        action = Action.Attacking;
        ultimateAttacking = true;
    }

    public void StartAttack()
    {
        invincible = false;
        action = Action.Attacking;
    } 

    public void EndAttack()
    {
        action = Action.Neutral;
        ultimateAttacking = false;
        attackManager.ResetCombo();
        rightShinCollider.isTrigger = false;
        rightFootCollider.isTrigger = false;
        rightHandCollider.isTrigger = false;
        rightForearmCollider.isTrigger = false;
    }

    public void EnableAttackInput()
    {
        attackManager.SetReadyForAttackInput(true);
    }

    public void DisableAttackInput()
    {
        attackManager.SetReadyForAttackInput(false);
    }

    public void EnableAttackAnimation()
    {
        attackManager.SetReadyForAttackAnimation(true);
    }

    private void AttackEnemy()
    {
        if (!otherFighterController.SuccessfullyBlocked(stance))
        {
            gameManager.DealDamageToFighter(currentAttackDamage, currentAttackType, !isPlayerOne);
        }
    }

    private void UltimateAttackEnemy()
    {
        if (!otherFighterController.SuccessfullyBlocked(stance))
        {
            gameManager.DealDamageToFighter(50, AttackType.KnockBack, !isPlayerOne);
            //attackManager.PerformUltimateAttack(isPlayerOne);
        }
    }

    public bool SuccessfullyBlocked(Stance attackerStance)
    {
        bool attackerIsCrouching = attackerStance == Stance.Crouching;
        bool isCrouching = stance == Stance.Crouching;
        bool successfullyBlocked = invincible || 
        (action == Action.Blocking && (isCrouching == attackerIsCrouching));
        if (!successfullyBlocked)
        {
            sparkParticles.Play();
        }
        return successfullyBlocked;
    }

    public bool SuccessfullyBlockedProjectile()
    {
        bool successfullyBlocked = invincible || action == Action.Blocking;
        if (!successfullyBlocked)
        {
            sparkParticles.Play();
        }
        return successfullyBlocked;
    }

    private void Crouch()
    {
        invincible = false;
        stance = Stance.Crouching;
    }

    private void Uncrouch()
    {
        stance = Stance.Standing;
    }

    private void Block()
    {
        invincible = false;
        action = Action.Blocking;
    }

    private void Unblock()
    {
        action = Action.Neutral;
    }

    private void GetUp()
    {
        stance = Stance.Standing;
        action = Action.Recovering;
        invincible = true;
        anim.Play("Get Up");
    }

    private void AttackMoveForward()
    {
        attackMoving = true;
        if (isPlayerOne)
        {
            attackingVelocity = new Vector3(moveSpeed, 0f, 0f);
        } else
        {
            attackingVelocity = new Vector3(-moveSpeed, 0f, 0f);
        }
    }

    private void StopAttackMoving()
    {
        attackMoving = false;
    }

    private void ChangeStance()
    {
        stance = Stance.ChangingStance;
    }

    private void ChangeBlock()
    {
        action = Action.ChangingBlock;
    }

    public float GetRightmostPosition()
    {
        float rightmostPosition = -1000f;
        foreach (Collider collider in GetComponentsInChildren<Collider>())  //May need to change when actual models are put in
        {
            float rightmostCollider = collider.transform.position.x + .05f;
            if (rightmostCollider > rightmostPosition)
            {
                rightmostPosition = rightmostCollider;
            }
        }

        return rightmostPosition;
    }

    public float GetLeftmostPosition()
    {
        float leftmostPosition = 1000f;
        foreach (Collider collider in GetComponentsInChildren<Collider>())   //May need to change when actual models are put in
        {
            float leftmostCollider = collider.transform.position.x - .05f;
            if (leftmostCollider < leftmostPosition)
            {
                leftmostPosition = leftmostCollider;
            }
        }

        return leftmostPosition;
    }

    private void TurnLegCollidersIntoTriggers()
    {
        leftShinCollider.isTrigger = true;
        leftFootCollider.isTrigger = true;
        leftThighCollider.isTrigger = true;
        rightShinCollider.isTrigger = true;
        rightFootCollider.isTrigger = true;
        rightThighCollider.isTrigger = true;
}

    private void TurnLegCollidersIntoColliders()
    {
        leftShinCollider.isTrigger = false;
        leftFootCollider.isTrigger = false;
        leftThighCollider.isTrigger = false;
        rightShinCollider.isTrigger = false;
        rightFootCollider.isTrigger = false;
        rightThighCollider.isTrigger = false;
    }

    public void ActivateDelayFrame()
    {
        delayFrame = true;
    }

    public string GetRandomIntroQuote()
    {
        return introQuotes[Random.Range(0, introQuotes.Length)];
    }

    public string GetRandomVictoryQuote()
    {
        return victoryQuotes[Random.Range(0, victoryQuotes.Length)];
    }

    public int GetHp()
    {
        return hp;
    }

    private void LandOnFloor()
    {
        if (stance == Stance.KnockedUp)
        {
            stance = Stance.KnockedDown;
            invincible = true;
            TurnLegCollidersIntoColliders();
            anim.Play("Flop");
        }
        else if (stance == Stance.Jumping)
        {
            stance = Stance.Standing;
            if (action == Action.Attacking)
            {
                anim.Rebind();
                attackManager.ResetCombo();
                //Maybe add in recovery time here
                action = Action.Neutral;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            LandOnFloor();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject == otherFighter)
        {
            collidingWithEnemy = true;
        }
        else if (other.transform.tag == "Floor")
        {
            LandOnFloor();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.gameObject == otherFighter)
        {
            collidingWithEnemy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.gameObject == otherFighter)
        {
            collidingWithEnemy = false;
        }
    }
}

public enum Stance
{
    Standing,
    Crouching,
    Jumping,
    ChangingStance,
    KnockedUp,
    KnockedDown
}

public enum Action
{
    Neutral,
    Attacking,
    Blocking,
    ChangingBlock,
    Recovering
}