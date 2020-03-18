using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool gameActive = false;
    private float time;
    private Canvas canvas;
    [SerializeField] Text timerText;
    [SerializeField] Text winText;
    [SerializeField] CameraMover mainCamera;
    [SerializeField] ZoomInCamera zoomInCamera;
    [SerializeField] float preFightTimer;
    private FighterContoller fighterOne;
    private FighterContoller fighterTwo;
    private bool preFight = false;
    private bool postFight = false;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        fighterOne = GameObject.FindGameObjectWithTag("Fighter1").GetComponent<FighterContoller>();
        fighterTwo = GameObject.FindGameObjectWithTag("Fighter2").GetComponent<FighterContoller>();
        fighterOne.Init();
        fighterTwo.Init();
        mainCamera.Init();
        zoomInCamera.Init();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive) //Fight is in progress
        {
            time -= Time.deltaTime;
            timerText.text = "" + (int)time;
            if ((int)time == 0)
            {
                gameActive = false;
                winText.enabled = true;
                winText.text = "TIME!";
            }
        } else if (preFight)    //Short time after intro but before fight
        {
            preFightTimer -= Time.deltaTime;
            if (preFightTimer <= 0f)
            {
                winText.enabled = false;
                preFight = false;
                gameActive = true;
            }
        } else if (postFight)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartGame();
            }
        }
    }

    public void StartGame()
    {
        fighterOne.ResetFighter();
        fighterTwo.ResetFighter();
        time = 99.9f;
        preFightTimer = 1f;
        timerText.text = "" + (int)time;
        StartFighterZoom();
    }

    public void StartFighterZoom()
    {
        canvas.enabled = false;
        zoomInCamera.EnableCamera();
        mainCamera.DisableCamera();
        zoomInCamera.StartCharacterIntro();
    }

    public void StartPreFight()
    {
        canvas.enabled = true;
        zoomInCamera.DisableCamera();
        mainCamera.EnableCamera();
        winText.enabled = true;
        winText.text = "FIGHT!";
        preFight = true;
    }

    public bool IsGameActive()
    {
        return gameActive;
    }

    public void DeactivateGame(bool fighterOneWins)
    {
        int fighterNumber = 1;
        if (!fighterOneWins)
        {
            fighterNumber = 2;
        }
        gameActive = false;
        winText.enabled = true;
        winText.text = "GAME!\nFIGHTER " + fighterNumber + " WINS!";
        postFight = true;
    }
}
