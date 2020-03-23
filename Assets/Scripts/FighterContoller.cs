using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterContoller : MonoBehaviour
{
    private bool isPlayerOne = true;
    [SerializeField] float moveSpeed = 1f;
    private GameObject otherFighter;
    private FighterContoller otherFighterController;
    private GameManager gameManager;
    private Rigidbody rb;
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 startingScale;
    private int hp; //Max is 100
    private int special; //Max is 50
    private FighterUI fighterUI;
    private bool isJumping;
    [SerializeField] BodyPartCollision rightShinCollider;
    [SerializeField] BodyPartCollision rightFootCollider;
    private Animator anim;
    private bool collidingWithEnemy;
    private bool kicking;
    private bool punching;
    private bool attacking;
    private bool recovering;
    private bool crouching;
    private bool blocking;

    private Material fighterMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGameActive()) {
            if (!recovering && !attacking)
            {
                Vector3 velocity = new Vector3(0f, rb.velocity.y);
                if (isJumping)
                {
                    velocity.x = rb.velocity.x;
                }

                if (isPlayerOne)
                {    
                    if (!isJumping) //Stuff that can be done when not in air
                    {
                        if (Input.GetKey(KeyCode.A))
                        {
                            velocity.x -= moveSpeed;
                        }
                        else if (Input.GetKey(KeyCode.D))
                        {
                            velocity.x += moveSpeed;
                        }

                        if (Input.GetKey(KeyCode.S) && !crouching)  //Crouching and uncrouching
                        {
                            anim.Play("Crouch");
                        }
                        else if (!Input.GetKey(KeyCode.S) && crouching)
                        {
                            anim.Play("Uncrouch");
                        }

                        if (Input.GetKey(KeyCode.Z) && !blocking) //Blocking
                        {
                            anim.Play("Block");
                        }
                        else if (!Input.GetKey(KeyCode.Z) && blocking)
                        {
                            anim.Play("Unblock");
                        }
                    }

                    if (blocking && Input.GetKey(KeyCode.W))
                    {
                        //Rising block
                    }

                    if (!crouching && !isJumping && !blocking)  //Standing
                    {
                        if (Input.GetKeyDown(KeyCode.Q))
                        {
                            attacking = true;
                            anim.Play("Kick");
                        }
                        else if (Input.GetKeyDown(KeyCode.E))
                        {
                            attacking = true;
                            anim.Play("Punch");
                        }
                        else if (Input.GetKeyDown(KeyCode.X)) 
                        {
                            special += 10;
                            if (special > 50)
                            {
                                special = 50;
                            }
                            fighterUI.UpdateSpecial(special);
                        }
                        else if (Input.GetKey(KeyCode.W))
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
                }
                    
                else
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        velocity.x -= moveSpeed;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        velocity.x += moveSpeed;
                    }

                    if (Input.GetKey(KeyCode.UpArrow) && !isJumping)
                    {
                        isJumping = true;
                        velocity.y = 3f;
                    }

                    if (Input.GetKeyDown(KeyCode.O))
                    {
                        attacking = true;
                        anim.Play("Kick");
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        attacking = true;
                        anim.Play("Punch");
                    }
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

    public void Init(GameObject otherFighter)
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        this.otherFighter = otherFighter;
        otherFighterController = otherFighter.GetComponent<FighterContoller>();

        if (isPlayerOne)
        {
            fighterUI = canvas.transform.GetChild(0).GetComponent<FighterUI>();
        }
        else
        {
            fighterUI = canvas.transform.GetChild(1).GetComponent<FighterUI>();
        }
        anim = GetComponent<Animator>();
        fighterUI.Init();

        startingPosition.Set(transform.position.x, transform.position.y, transform.position.z);
        startingRotation.Set(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        startingScale.Set(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void SetAsPlayerOne(bool isPlayerOne)
    {
        this.isPlayerOne = isPlayerOne;
    }

    public void ResetFighter()
    {
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
    }

    public void TakeDamage(int damage)
    {
        attacking = false;
        kicking = false;
        punching = false;
        recovering = true;
        crouching = false;
        blocking = false;
        anim.StopPlayback();
        

        hp -= damage;
        if (hp > 0)
        {
            anim.Play("Get Hit");
        }
        else
        {
            anim.Play("Die");
            hp = 0;
            gameManager.DeactivateGame(!isPlayerOne);
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
    }

    public void EndKick()
    {
        kicking = false;
    }

    private void KickEnemy()
    {
        //if (rightShinCollider.IsCollidingWithEnemy() || rightFootCollider.IsCollidingWithEnemy())
        if (collidingWithEnemy)
        {
            DamageEnemy(10);
            kicking = false;
        }
    }

    private void StartPunch()
    {
        punching = true;
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
            punching = false;
        }
    }

    public void EndAttack()
    {
        attacking = false;
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
            isJumping == this.isJumping &&
            crouching == this.crouching);
    }

    private void Crouch()
    {
        crouching = true;
    }

    private void Uncrouch()
    {
        crouching = false;
    }

    private void Block()
    {
        blocking = true;
    }

    private void Unblock()
    {
        blocking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            isJumping = false;
        }
        else if (collision.transform.gameObject == otherFighter)
        {
            collidingWithEnemy = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.gameObject == otherFighter)
        {
            collidingWithEnemy = false;
        }
    }
}
