using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    ControllerInput controllerInput;
    private bool isPlayerOne = true;
    private bool paused = false;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] GameObject projectile;
    private AttackManager attackManager;
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
    private bool isJumping;
    [SerializeField] Collider leftShinCollider;
    [SerializeField] Collider leftFootCollider;
    [SerializeField] Collider rightShinCollider;
    [SerializeField] Collider rightFootCollider;
    [SerializeField] Collider rightHandCollider;
    [SerializeField] Collider rightForearmCollider;
    [SerializeField] ParticleSystem sparkParticles;
    private Animator anim;
    private bool collidingWithEnemy;
    private bool attacking;
    private bool ultimateAttacking;
    private bool attackMoving;
    private bool recovering;
    private bool crouching;
    private bool blocking;
    private bool changingStance;    //Set to true when starting crouch or block to prevent jumping
    private bool inHitFrame;
    private bool knockedUp;
    private bool knockedDown;
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
        attackingVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGameActive()) {

            if (controllerInput.GetStartButtonDown())
            {
                paused = true;
                gameManager.PauseGame(isPlayerOne);
            }
            if (isPlayerOne && Input.GetKeyDown(KeyCode.B))  //For debugging purposes
            {
                print("attacking: " + attacking);
                print("recovering: " + recovering);
                print("crouching: " + crouching);
                print("blocking: " + blocking);
                print("jumping: " + isJumping);
                print("z: " + Input.GetKey(KeyCode.Z));
            }

            if (!recovering && !blocking && !changingStance && !knockedDown)
            {
                if (controllerInput.GetLeftActionButtonDown())
                {
                    attackManager.Punch();
                }
                else if (controllerInput.GetBottomActionButtonDown())
                {
                    attackManager.Kick();
                }
                else if (controllerInput.GetTopActionButtonDown())
                {
                    attackManager.RangedAttack();
                }
                else if (!controllerInput.GetRightBumper() && controllerInput.GetRightActionButtonDown())
                {
                    attackManager.SpecialAttack();
                }
                else if (controllerInput.GetRightBumper() && controllerInput.GetRightActionButtonDown())
                {
                    attackManager.Ultimate();
                }

                attackManager.PerformComboAttackIfQueued();
            }

            if (!recovering && !attacking && !knockedDown)
            {
                Vector3 velocity = new Vector3(0f, rb.velocity.y);
                if (isJumping)
                {
                    velocity.x = rb.velocity.x;
                }

                if (!isJumping) //Blocking, moving, and crouching inputs
                {
                    if (controllerInput.GetXAxisLeft()) //Movement
                    {
                        velocity.x -= moveSpeed;
                    }
                    else if (controllerInput.GetXAxisRight())
                    {
                        velocity.x += moveSpeed;
                    }

                    if (blocking)
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
                        }
                    }

                    if (crouching)
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

                if (!blocking && !changingStance)  
                {
                    if (!crouching && !isJumping)   //Standing
                    {
                        if (controllerInput.GetLeftBumperDown())
                        {
                            attacking = true;
                            anim.Play("Taunt");
                        }
                        else if (controllerInput.GetYAxisUp())
                        {
                            isJumping = true;
                            velocity.y = 3f;
                        }
                    }
                    else if (crouching)
                    {
                        if (controllerInput.GetRightActionButtonDown())
                        {
                            attacking = true;
                            anim.Play("Crouch Kick");
                        }
                        else if (controllerInput.GetBottomActionButtonDown())
                        {
                            attacking = true;
                            anim.Play("Crouch Punch");
                        }
                        else if (controllerInput.GetLeftActionButtonDown())
                        {
                            attacking = true;
                            anim.Play("Crouch Shoot");
                        }
                    }
                    else if (isJumping)
                    {
                        if (controllerInput.GetRightActionButtonDown())
                        {
                            attacking = true;
                            anim.Play("Jump Kick");
                        }
                        else if (controllerInput.GetBottomActionButtonDown())
                        {
                            attacking = true;
                            anim.Play("Jump Punch");
                        }
                        else if (controllerInput.GetLeftActionButtonDown())
                        {
                            attacking = true;
                            anim.Play("Jump Shoot");
                        }
                    }

                }

                rb.velocity = velocity;
            }
            else if (attackMoving)
            {
                rb.velocity = attackingVelocity;
            } 
            else if (!recovering && knockedDown)
            {
                if (controllerInput.GetYAxisUp() ||
                    !isPlayerOne)   //Remove this or statement, for debugging only
                {
                    knockedDown = false;
                    recovering = true;
                    invincible = true;
                    anim.Play("Get Up");
                }
            }

            if (inHitFrame)
            {
                CheckForHit();
            }
        }
        else if (paused)
        {
            if (controllerInput.GetStartButtonDown())
            {
                gameManager.ResumeGame();
            }
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
        anim.Rebind();
        anim.Play("Default");
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        transform.localScale = startingScale;
        hp = 100;
        special = 0;
        fighterUI.UpdateHp(hp);
        fighterUI.UpdateSpecial(special);
        isJumping = false;
        collidingWithEnemy = false;
        inHitFrame = false;
        attacking = false;
        ultimateAttacking = false;
        recovering = false;
        crouching = false;
        blocking = false;
        changingStance = false;
        knockedUp = false;
        knockedDown = false;
        invincible = false;
    }

    public void TakeDamage(int damage, AttackType attackType)
    {
        EndAttack();
        recovering = true;
        inHitFrame = false;
        crouching = false;
        blocking = false;
        changingStance = false;
        attackMoving = false;
        anim.Rebind();  //Stops playback on all layers

        hp -= damage;
        if (hp > 0)
        {
            if (knockedUp)
            {
                rb.velocity = new Vector3(rb.velocity.x, 5f, rb.velocity.z);
                anim.Play("Juggle");
            }
            else if (knockedDown)
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
                    knockedDown = true;
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
                    float velX = -.2f;
                    if (!isPlayerOne)
                    {
                        velX = .2f;
                    }
                    rb.velocity = new Vector3(velX, 5f, rb.velocity.z);
                    leftShinCollider.isTrigger = false;
                    leftFootCollider.isTrigger = false;
                    rightShinCollider.isTrigger = false;
                    rightFootCollider.isTrigger = false;
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

    public void KnockUp()
    {
        knockedUp = true;
    }

    public void Recover()
    {
        recovering = false;
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
                SpecialAttackEnemy();
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
        attacking = false;
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
        attacking = true;
        ultimateAttacking = true;
    }

    public void StartAttack()
    {
        attacking = true;
    } 

    public void EndAttack()
    {
        attacking = false;
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
        if (!otherFighterController.SuccessfullyBlocked(crouching))
        {
            gameManager.DealDamageToFighter(currentAttackDamage, currentAttackType, !isPlayerOne);
        }
    }

    private void SpecialAttackEnemy()
    {
        if (!otherFighterController.SuccessfullyBlocked(crouching))
        {
            gameManager.DealDamageToFighter(50, AttackType.KnockBack, !isPlayerOne);
            //attackManager.PerformUltimateAttack(!isPlayerOne);
        }
    }

    public bool SuccessfullyBlocked(bool crouching)
    {
        bool successfullyBlocked = invincible || 
        (blocking && crouching == this.crouching);
        if (!successfullyBlocked)
        {
            sparkParticles.Play();
        }
        return successfullyBlocked;
    }

    public bool SuccessfullyBlockedProjectile()
    {
        bool successfullyBlocked = invincible || blocking;
        if (!successfullyBlocked)
        {
            sparkParticles.Play();
        }
        return successfullyBlocked;
    }

    private void Crouch()
    {
        crouching = true;
        changingStance = false;
    }

    private void Uncrouch()
    {
        crouching = false;
    }

    private void Block()
    {
        blocking = true;
        changingStance = false;
    }

    private void Unblock()
    {
        blocking = false;
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
        changingStance = true;
    }

    public string GetRandomIntroQuote()
    {
        return introQuotes[Random.Range(0, introQuotes.Length)];
    }

    public string GetRandomVictoryQuote()
    {
        return victoryQuotes[Random.Range(0, victoryQuotes.Length)];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            isJumping = false;
            if (knockedUp)
            {
                leftShinCollider.isTrigger = false;
                leftFootCollider.isTrigger = false;
                rightShinCollider.isTrigger = false;
                rightFootCollider.isTrigger = false;
                knockedUp = false;
                knockedDown = true;
                invincible = true;
                anim.Play("Flop");
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject == otherFighter)
        {
            collidingWithEnemy = true;
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