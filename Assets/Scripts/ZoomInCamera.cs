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
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        camera = GetComponent<Camera>();
        anim = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    public void StartCharacterIntro()
    {
        Vector3 parentPos = transform.parent.position;
        transform.parent.position = new Vector3(0f, parentPos.y, parentPos.z);
        anim.Play("Fighter1 Intro");
        gameManager.SetIntroQuote(true);
    }

    private void SetSecondIntroQuote()
    {
        gameManager.SetIntroQuote(false);
    }

    public void StartFighterOneVictory(float fighterOneX)
    {
        Vector3 parentPos = transform.parent.position;
        transform.parent.position = new Vector3(fighterOneX + .9f, parentPos.y, parentPos.z);
        anim.Play("Fighter1 Victory");
        gameManager.SetVictoryQuote(true);
    }

    public void StartFighterTwoVictory(float fighterTwoX)
    {
        Vector3 parentPos = transform.parent.position;
        transform.parent.position = new Vector3(fighterTwoX - .9f, parentPos.y, parentPos.z);
        anim.Play("Fighter2 Victory");
        gameManager.SetVictoryQuote(false);
    }

    private void DisplayRematchMenu()
    {
        gameManager.DisplayRematchMenu();
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
        anim.Rebind();
        camera.enabled = false;
    }
}
