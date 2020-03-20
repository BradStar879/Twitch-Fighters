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
    [SerializeField] Text fighterOneName;
    [SerializeField] Text fighterTwoName;
    [SerializeField] CameraMover mainCamera;
    [SerializeField] ZoomInCamera zoomInCamera;
    [SerializeField] float preFightTimer;
    private FighterContoller fighterOne;
    private FighterContoller fighterTwo;
    private bool preFight = false;
    private bool postFight = false;

    private readonly Vector3 fighterOneStartingPosition = new Vector3(-.9f, .6f, -2.9f);
    private readonly Vector3 fighterTwoStartingPosition = new Vector3(.9f, .6f, -2.9f);
    private readonly Quaternion fighterOneStartingRotation = Quaternion.Euler(Vector3.zero);
    private readonly Quaternion fighterTwoStartingRotation = Quaternion.Euler(0f, 180f, 0f);

    [SerializeField] GameObject redFighter;
    [SerializeField] GameObject blueFighter;
    [SerializeField] GameObject greenFighter;
    [SerializeField] GameObject yellowFighter;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        fighterOneName.text = "" + GameData.GetFighterOneCharacter();
        fighterTwoName.text = "" + GameData.GetFighterTwoCharacter();
        GameObject fighterOneToClone = DecideFighter(GameData.GetFighterOneCharacter());
        GameObject fighterTwoToClone = DecideFighter(GameData.GetFighterTwoCharacter());

        GameObject fighterOneClone = Instantiate(fighterOneToClone, fighterOneStartingPosition, fighterOneStartingRotation);
        GameObject fighterTwoClone = Instantiate(fighterTwoToClone, fighterTwoStartingPosition, fighterTwoStartingRotation);
        fighterOneClone.SetActive(true);
        fighterTwoClone.SetActive(true);
        fighterOne = fighterOneClone.GetComponent<FighterContoller>();
        fighterTwo = fighterTwoClone.GetComponent<FighterContoller>();
        fighterOne.SetAsPlayerOne(true);
        fighterTwo.SetAsPlayerOne(false);
        fighterOne.Init(fighterTwoClone);
        fighterTwo.Init(fighterOneClone);
        mainCamera.Init(fighterOneClone, fighterTwoClone);
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

    private GameObject DecideFighter(MainMenuScript.characters character)
    {
        switch (character)
        {
            case MainMenuScript.characters.red:
                return redFighter;
            case MainMenuScript.characters.blue:
                return blueFighter;
            case MainMenuScript.characters.green:
                return greenFighter;
            case MainMenuScript.characters.yellow:
                return yellowFighter;
            default:
                return null;
        }
    }
}
