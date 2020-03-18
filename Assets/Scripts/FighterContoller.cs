using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterContoller : MonoBehaviour
{
    [SerializeField] bool isPlayerOne = true;
    [SerializeField] float moveSpeed = 1f;
    private FighterContoller otherFighter;
    private GameManager gameManager;
    private Rigidbody rb;
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 startingScale;
    private int hp;
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

                if (isPlayerOne)
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        velocity.x -= moveSpeed;
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        velocity.x += moveSpeed;
                    }

                    if (Input.GetKey(KeyCode.W) && !isJumping)
                    {
                        isJumping = true;
                        velocity.y = 3f;
                    }

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

                    if (Input.GetKey(KeyCode.Space) && !isJumping)
                    {
                        isJumping = true;
                        velocity.y = 3f;
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

    public void Init()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        if (isPlayerOne)
        {
            otherFighter = GameObject.FindGameObjectWithTag("Fighter2").GetComponent<FighterContoller>();
            fighterUI = canvas.transform.GetChild(0).GetComponent<FighterUI>();
        }
        else
        {
            otherFighter = GameObject.FindGameObjectWithTag("Fighter1").GetComponent<FighterContoller>();
            fighterUI = canvas.transform.GetChild(1).GetComponent<FighterUI>();
        }
        anim = GetComponent<Animator>();
        fighterUI.Init();

        startingPosition.Set(transform.position.x, transform.position.y, transform.position.z);
        startingRotation.Set(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        startingScale.Set(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void ResetFighter()
    {
        anim.Play("Default");
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        transform.localScale = startingScale;
        hp = 100;
        fighterUI.UpdateHp(hp);
        isJumping = false;
        collidingWithEnemy = false;
        kicking = false;
        punching = false;
        attacking = false;
        recovering = false;
    }

    public void TakeDamage(int damage)
    {
        attacking = false;
        kicking = false;
        punching = false;
        recovering = true;
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
        otherFighter.TakeDamage(damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            isJumping = false;
        }
        else if (collision.transform.tag == "Fighter2")
        {
            collidingWithEnemy = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Fighter2")
        {
            collidingWithEnemy = false;
        }
    }
}
