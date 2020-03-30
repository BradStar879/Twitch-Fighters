using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private GameManager gameManager;
    private float duration;
    [SerializeField] float speed;
    [SerializeField] int damage;

    public void Init(bool isFighterOne)
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        duration = 10f;
        if (!isFighterOne)
        {
            speed *= -1f;
        }
        GetComponent<Rigidbody>().velocity = new Vector3(speed, 0f, 0f);
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        FighterContoller fighterController = other.transform.root.GetComponent<FighterContoller>();
        if (fighterController != null) //Hit fighter 
        {
            gameManager.DealDamageToFighter(damage, fighterController.IsPlayerOne());
            Destroy(gameObject);
        }

    }

}
