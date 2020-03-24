using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool gameActive = false;
    private float time;
    private Canvas canvas;
    [SerializeField] bool debugMode = false;
    [SerializeField] Text timerText;
    [SerializeField] Text winText;
    [SerializeField] Text fighterOneName;
    [SerializeField] Text fighterTwoName;
    [SerializeField] CameraMover mainCamera;
    [SerializeField] ZoomInCamera zoomInCamera;
    [SerializeField] float preFightTimer = 0f;
    private float postRoundTimer;
    [SerializeField] int roundsToWin = 3;
    private FighterContoller fighterOne;
    private FighterContoller fighterTwo;
    private FighterUI fighterOneUI;
    private FighterUI fighterTwoUI;
    private bool preFight = false;
    private bool postRound = false;
    private bool postFight = false;
    private int fighterOneWins;
    private int fighterTwoWins;

    private readonly Vector3 fighterOneStartingPosition = new Vector3(-.9f, .6f, -2.9f);
    private readonly Vector3 fighterTwoStartingPosition = new Vector3(.9f, .6f, -2.9f);
    private readonly Quaternion fighterOneStartingRotation = Quaternion.Euler(Vector3.zero);
    private readonly Quaternion fighterTwoStartingRotation = Quaternion.Euler(0f, 180f, 0f);

    [SerializeField] GameObject redFighter;
    [SerializeField] GameObject blueFighter;
    [SerializeField] GameObject greenFighter;
    [SerializeField] GameObject yellowFighter;

    private int damageToDealToPlayerOne = 0;
    private int damageToDealToPlayerTwo = 0;
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
        fighterOneUI = fighterOne.GetFighterUI();
        fighterTwoUI = fighterTwo.GetFighterUI();
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
                postFight = true;
                winText.text = "TIME!";
            }
        }
        else if (preFight)    //Short time after intro but before fight
        {
            preFightTimer -= Time.deltaTime;
            if (preFightTimer <= 0f)
            {
                winText.enabled = false;
                preFight = false;
                gameActive = true;
            }
        }
        else if (postRound) //After round but not enough wins to end game
        {
            postRoundTimer -= Time.deltaTime;
            if (postRoundTimer <= 0f)
            {
                postRound = false;
                StartRound();
            }
        }
        else if (postFight)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartGame();
            }
        }
    }

    private void LateUpdate()
    {
        if (damageToDealToPlayerOne > 0)
        {
            fighterOne.TakeDamage(damageToDealToPlayerOne);
            damageToDealToPlayerOne = 0;
        }
        if (damageToDealToPlayerTwo > 0)
        {
            fighterTwo.TakeDamage(damageToDealToPlayerTwo);
            damageToDealToPlayerTwo = 0;
        }
    }

    public void StartGame()
    {
        gameActive = false;
        fighterOne.ResetFighter();
        fighterTwo.ResetFighter();
        fighterOneWins = 0;
        fighterTwoWins = 0;
        if (!debugMode)
        {
            StartFighterZoom();
        } 
        else 
        {
            StartRound();   
        }
    }

    public void StartRound()
    {
        zoomInCamera.DisableCamera();
        mainCamera.EnableCamera();
        canvas.enabled = true;
        fighterOne.ResetFighter();
        fighterTwo.ResetFighter();
        winText.enabled = true;
        winText.text = "FIGHT!";
        time = 99.9f;
        timerText.text = "" + (int)time;
        preFightTimer = 1f;
        preFight = true;
    }

    public void StartFighterZoom()
    {
        canvas.enabled = false;
        zoomInCamera.EnableCamera();
        mainCamera.DisableCamera();
        zoomInCamera.StartCharacterIntro();
    }

    public bool IsGameActive()
    {
        return gameActive;
    }

    public void EndRound(bool fighterOneWinsRound)
    {
        gameActive = false;
        winText.enabled = true;
        if (fighterOneWinsRound)
        {
            fighterOneWins++;
            fighterOneUI.UpdateWins(fighterOneWins);
            if (fighterOneWins == roundsToWin)
            {
                winText.text = "GAME!\nFIGHTER 1 WINS!";
                postFight = true;
            }
            else
            {
                winText.text = "FIGHTER 1 WON THE ROUND!";
                postRound = true;
                postRoundTimer = 2f;
            }
        }
        else
        {
            fighterTwoWins++;
            fighterTwoUI.UpdateWins(fighterTwoWins);
            if (fighterTwoWins == roundsToWin)
            {
                winText.text = "GAME!\nFIGHTER 2 WINS!";
                postFight = true;
            }
            else
            {
                winText.text = "FIGHTER 2 WON THE ROUND!";
                postRound = true;
                postRoundTimer = 2f;
            }
        }
    }

    public void DealDamageToFighter(int damage, bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            damageToDealToPlayerOne += damage;
        } else
        {
            damageToDealToPlayerTwo += damage;
        }
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
