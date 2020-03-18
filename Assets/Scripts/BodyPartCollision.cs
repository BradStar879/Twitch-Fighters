using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartCollision : MonoBehaviour
{
    private bool collidingWithEnemy = false;
    [SerializeField] bool playerOne;
    private string enemyTag;
    // Start is called before the first frame update
    void Start()
    {
        if (playerOne)
        {
            enemyTag = "Fighter2";
        }
        else
        {
            enemyTag = "Fighter1";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsCollidingWithEnemy()
    {
        return collidingWithEnemy;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == enemyTag)
        {
            collidingWithEnemy = true;
            print("enter");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == enemyTag)
        {
            collidingWithEnemy = false;
            print("exit");
        }
    }
}
