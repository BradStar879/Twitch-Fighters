using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInCamera : MonoBehaviour
{
    private Camera camera;
    private Animator anim;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start() {
    
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        camera = GetComponent<Camera>();
        anim = GetComponent<Animator>();
    }

    public void StartCharacterIntro()
    {
        anim.Play("Fighter1 Intro");
    }

    public void StartRound()
    {
        gameManager.StartRound();
    }

    public void EnableCamera()
    {
        camera.enabled = true;
    }

    public void DisableCamera()
    {
        camera.enabled = false;
    }
}
