using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    ControllerInput controllerInput;
    private bool isPlayerOne = true;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] GameObject projectile;
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
    private Animator anim;
    private bool collidingWithEnemy;
    private bool kicking;
    private bool punching;
    private bool attacking;
    private bool recovering;
    private bool crouching;
    private bool blocking;
    private bool risingBlocking;
    private bool changingStance;    //Set to true when starting crouch or block to prevent jumping

    private Material fighterMaterial;

    //To be populated by child classes
    protected string[] introQuotes;
    protected string[] victoryQuotes;


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
        fighterUI.Init();

        startingPosition.Set(transform.position.x, transform.position.y, transform.position.z);
        startingRotation.Set(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        startingScale.Set(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGameActive()) {
            if(isPlayerOne && Input.GetKeyDown(KeyCode.B))  //For debugging purposes
            {
                print("kicking: " + kicking);
                print("punching: " + punching);
                print("attacking: " + attacking);
                print("recovering: " + recovering);
                print("crouching: " + crouching);
                print("blocking: " + blocking);
                print("jumping: " + isJumping);
                print("risingBlocking: " + risingBlocking);
                print("z: " + Input.GetKey(KeyCode.Z));
            }
            if (!recovering && !attacking)
            {
                Vector3 velocity = new Vector3(0f, rb.velocity.y);
                if (isJumping)
                {
                    velocity.x = rb.velocity.x;
                }

                if (!isJumping) //Stuff that can be done when not in air
                {
                    if (controllerInput.GetXAxisLeft())
                    {
                        velocity.x -= moveSpeed;
                    }
                    else if (controllerInput.GetXAxisRight())
                    {
                        velocity.x += moveSpeed;
                    }

                    if (Input.GetKeyDown(KeyCode.Z) && !blocking) //Blocking
                    {
                        anim.Play("Block");
                    }
                    else if (controllerInput.GetYAxisUp() && !crouching && blocking && !risingBlocking)
                    {
                        anim.Play("Rising Block");
                    }
                    else if (!controllerInput.GetYAxisUp() && risingBlocking)
                    {
                        anim.Play("Lower Block");
                    }
                    else if (!Input.GetKey(KeyCode.Z) && risingBlocking)
                    {
                        print("bbb");
                        anim.Play("Rising Unblock");
                    }
                    else if (!Input.GetKey(KeyCode.Z) && blocking)
                    {
                        anim.Play("Unblock");
                    }

                    if (!risingBlocking)
                    {
                        if (controllerInput.GetYAxisDown() && !crouching)  //Crouching and uncrouching
                        {
                            anim.Play("Crouch");
                        }
                        else if (!controllerInput.GetYAxisDown() && crouching)
                        {
                            anim.Play("Uncrouch");
                        }
                    }

                }

                if (!crouching && !isJumping && !blocking && !risingBlocking && !changingStance)  //Standing
                {
                    if (controllerInput.GetRightActionButtonDown())
                    {
                        attacking = true;
                        anim.Play("Kick");
                    }
                    else if (controllerInput.GetBottomActionButtonDown())
                    {
                        attacking = true;
                        anim.Play("Punch");
                    }
                    else if (controllerInput.GetLeftActionButtonDown())
                    {
                        attacking = true;
                        anim.Play("Shoot");
                    }
                    else if (controllerInput.GetTopActionButtonDown()) 
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
                    //Add crouching attacks
                } 
                else if (isJumping)
                {
                    //Add jumping attacks
                }

                rb.velocity = velocity;
            }

            if (kicking)
            {
                KickEnemy();
            }
            else if (punching)
            {
                PunchEnemy();
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

    public bool IsPlayerOne()
    {
        return isPlayerOne;
    }

    public void ResetFighter()
    {
        anim.StopPlayback();
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
        kicking = false;
        punching = false;
        attacking = false;
        recovering = false;
        crouching = false;
        blocking = false;
        risingBlocking = false;
        changingStance = false;
    }

    public void TakeDamage(int damage)
    {
        EndAttack();
        kicking = false;
        punching = false;
        recovering = true;
        crouching = false;
        blocking = false;
        risingBlocking = false;
        changingStance = false;
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

    public void StartKick()
    {
        kicking = true;
        rightShinCollider.isTrigger = true;
        rightFootCollider.isTrigger = true;
    }

    public void EndKick()
    {
        kicking = false;
        collidingWithEnemy = false;
    }

    private void KickEnemy()
    {
        if (collidingWithEnemy)
        {
            DamageEnemy(30);
            kicking = false;
        }
    }

    private void StartPunch()
    {
        punching = true;
        rightHandCollider.isTrigger = true;
    }

    private void EndPunch()
    {
        punching = false;
    }

    private void PunchEnemy()
    {
        if (collidingWithEnemy)
        {
            DamageEnemy(7);
            EndPunch();
        }
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

    public void EndAttack()
    {
        attacking = false;
        rightShinCollider.isTrigger = false;
        rightFootCollider.isTrigger = false;
        rightHandCollider.isTrigger = false;
    }

    private void DamageEnemy(int damage)
    {
        if (!otherFighterController.SuccessfullyBlocked(isJumping, crouching))
        {
            gameManager.DealDamageToFighter(damage, !isPlayerOne);
        }
        
    }

    public bool SuccessfullyBlocked(bool isJumping, bool crouching)
    {
        return (blocking &&
            isJumping == this.risingBlocking &&
            crouching == this.crouching);
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

    private void RisingBlock()
    {
        risingBlocking = true;
    }

    private void LowerBlock()
    {
        risingBlocking = false;
    }

    private void Unblock()
    {
        blocking = false;
        risingBlocking = false;
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

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.gameObject == otherFighter)
        {
            collidingWithEnemy = false;
        }
    }
}
