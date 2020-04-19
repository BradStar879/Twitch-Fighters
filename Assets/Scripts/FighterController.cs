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
    [SerializeField] Collider rightShinCollider;
    [SerializeField] Collider rightFootCollider;
    [SerializeField] Collider rightHandCollider;
    [SerializeField] Collider rightForearmCollider;
    [SerializeField] ParticleSystem sparkParticles;
    private Animator anim;
    private bool collidingWithEnemy;
    private bool attacking;
    private bool attackMoving;
    private bool recovering;
    private bool crouching;
    private bool blocking;
    private bool changingStance;    //Set to true when starting crouch or block to prevent jumping
    private bool inHitFrame;
    private Vector3 attackingVelocity;
    private int currentAttackDamage;

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

            if (!recovering && !blocking && !changingStance)
            {
                if (controllerInput.GetLeftActionButtonDown())
                {
                    attackManager.Punch();
                }

                attackManager.PerformComboAttackIfQueued();
            }

            if (!recovering && !attacking)
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
                        if (controllerInput.GetBottomActionButtonDown())
                        {
                            attacking = true;
                            anim.Play("Kick");
                        }
                        else if (controllerInput.GetTopActionButtonDown())
                        {
                            attacking = true;
                            anim.Play("Shoot");
                        }
                        else if (controllerInput.GetLeftBumperDown())
                        {
                            attacking = true;
                            anim.Play("Taunt");
                        }
                        else if (special == 50 && controllerInput.GetRightBumper() && controllerInput.GetRightActionButtonDown())
                        {
                            //Ultimate move here
                        }
                        else if (controllerInput.GetRightActionButtonDown())
                        {
                            //Special move here
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
        recovering = false;
        crouching = false;
        blocking = false;
        changingStance = false;
    }

    public void TakeDamage(int damage)
    {
        EndAttack();
        inHitFrame = false;
        recovering = true;
        crouching = false;
        blocking = false;
        changingStance = false;
        attackMoving = false;
        anim.Rebind();  //Stops playback on all layers

        hp -= damage;
        if (hp > 0)
        {
            anim.Play("Get Hit");
        }
        else
        {
            anim.Play("Die");
            hp = 0;
            gameManager.EndRound(!isPlayerOne);
        } 
        fighterUI.UpdateHp(hp);
    }

    public void Recover()
    {
        recovering = false;
    }

    public void SetAttackDamage(int attackDamage)
    {
        currentAttackDamage = attackDamage;
    }

    public void EndHitFrame()
    {
        inHitFrame = false;
    }

    private void CheckForHit()
    {
        if (collidingWithEnemy)
        {
            AttackEnemy(currentAttackDamage);
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

    private void ChargeSpecial()
    {
        attacking = false;
        special += 10;
        if (special > 50)
        {
            special = 50;
        }
        fighterUI.UpdateSpecial(special);
    }

    public void StartAttack()
    {
        attacking = true;
    } 

    public void EndAttack()
    {
        attacking = false;
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

    private void AttackEnemy(int damage)
    {
        if (!otherFighterController.SuccessfullyBlocked(isJumping, crouching))
        {
            gameManager.DealDamageToFighter(damage, !isPlayerOne);
        }
    }

    public bool SuccessfullyBlocked(bool isJumping, bool crouching)
    {
        bool successfullyBlocked = blocking &&
            crouching == this.crouching;
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
