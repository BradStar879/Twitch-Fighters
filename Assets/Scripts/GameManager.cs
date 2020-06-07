using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameActive = false;
    private float time;
    private int roundedTime;
    private bool chargeSpecial;
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
    private FighterController fighterOne;
    private FighterController fighterTwo;
    private FighterUI fighterOneUI;
    private FighterUI fighterTwoUI;
    private bool preFight = false;
    private bool postRound = false;
    private bool postFight = false;
    private int fighterOneWins;
    private int fighterTwoWins;

    private GameObject fighterOneUiObject;
    private GameObject fighterTwoUiObject;
    private GameObject gameUiObject;
    private Text fighterSpeechText;
    private GameObject rematchMenuUiObject;
    private GameObject pauseMenuObject;
    private bool playerOnePaused;

    private readonly Vector3 fighterOneStartingPosition = new Vector3(-.9f, .6f, -2.9f);
    private readonly Vector3 fighterTwoStartingPosition = new Vector3(.9f, .6f, -2.9f);
    private readonly Quaternion fighterOneStartingRotation = Quaternion.Euler(Vector3.zero);
    private readonly Quaternion fighterTwoStartingRotation = Quaternion.Euler(0f, 180f, 0f);

    private Vector3 fighterOneSavedVelocity;
    private Vector3 fighterTwoSavedVelocity;

    [SerializeField] GameObject willNeffFighter;
    [SerializeField] GameObject hasanAbiFighter;
    [SerializeField] GameObject nesuaFighter;
    [SerializeField] GameObject redFighter;


    private int damageToDealToPlayerOne = 0;
    private AttackType typeOfDamageToDealToPlayerOne = AttackType.Flinch;
    private int damageToDealToPlayerTwo = 0;
    private AttackType typeOfDamageToDealToPlayerTwo = AttackType.Flinch;

    private MenuNavigation menuNavigation;

    void Start()
    {
        Physics.gravity = new Vector3(0, -7f, 0);
        gameObject.GetComponent<ControllerManager>().Init();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        fighterOneUiObject = canvas.transform.GetChild(0).gameObject;
        fighterTwoUiObject = canvas.transform.GetChild(1).gameObject;
        gameUiObject = canvas.transform.GetChild(2).gameObject;
        fighterSpeechText = canvas.transform.GetChild(3).gameObject.GetComponent<Text>();
        rematchMenuUiObject = canvas.transform.GetChild(4).gameObject;
        pauseMenuObject = canvas.transform.GetChild(5).gameObject;
        playerOnePaused = false;
        fighterOneName.text = "" + GameData.GetFighterOneCharacter();
        fighterTwoName.text = "" + GameData.GetFighterTwoCharacter();
        GameObject fighterOneToClone = DecideFighter(GameData.GetFighterOneCharacter());
        GameObject fighterTwoToClone = DecideFighter(GameData.GetFighterTwoCharacter());

        GameObject fighterOneClone = Instantiate(fighterOneToClone, fighterOneStartingPosition, fighterOneStartingRotation);
        GameObject fighterTwoClone = Instantiate(fighterTwoToClone, fighterTwoStartingPosition, fighterTwoStartingRotation);
        fighterOneClone.SetActive(true);
        fighterTwoClone.SetActive(true);
        fighterOne = fighterOneClone.GetComponent<FighterController>();
        fighterTwo = fighterTwoClone.GetComponent<FighterController>();
        fighterOne.Init(true, fighterTwoClone);
        fighterTwo.Init(false, fighterOneClone);
        fighterOneUI = fighterOne.GetFighterUI();
        fighterTwoUI = fighterTwo.GetFighterUI();
        mainCamera.Init(fighterOneClone, fighterTwoClone);
        zoomInCamera.Init();
        menuNavigation = GetComponent<MenuNavigation>();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive) //Fight is in progress
        {
            time -= Time.deltaTime;
            if (roundedTime != (int)time)   //One second has passed
            {
                roundedTime = (int)time;
                timerText.text = "" + roundedTime;
                if (chargeSpecial)  //Charge both players' specials by 1 every other second
                {
                    fighterOne.ChargeSpecial(1);
                    fighterTwo.ChargeSpecial(1);
                }
                chargeSpecial = !chargeSpecial;
            }
            
            if (roundedTime == 0)
            {
                EndRound(true);
                EndRound(false);
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
                if (!DetermineWinner()) 
                {
                    StartRound();
                }
            }
        }
        else if (postFight) //Only happens in case of match tie
        {
            postRoundTimer -= Time.deltaTime;
            if (postRoundTimer <= 0f)
            {
                postFight = false;
                DisplayRematchMenu();
            }
        }
    }

    private void LateUpdate()
    {
        if (damageToDealToPlayerOne > 0)
        {
            fighterOne.TakeDamage(damageToDealToPlayerOne, typeOfDamageToDealToPlayerOne);
            damageToDealToPlayerOne = 0;
            typeOfDamageToDealToPlayerOne = AttackType.Flinch;
        }
        if (damageToDealToPlayerTwo > 0)
        {
            fighterTwo.TakeDamage(damageToDealToPlayerTwo, typeOfDamageToDealToPlayerTwo);
            damageToDealToPlayerTwo = 0;
            typeOfDamageToDealToPlayerTwo = AttackType.Flinch;
        }
    }

    public int GetRoundsToWin()
    {
        return roundsToWin;
    }

    public void StartGame()
    {
        menuNavigation.DeactivateMenuBothPlayers();
        gameActive = false;
        rematchMenuUiObject.SetActive(false);
        fighterOne.ResetFighter();
        fighterTwo.ResetFighter();
        fighterOneUI.ResetWinCircles();
        fighterTwoUI.ResetWinCircles();
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
        fighterOneUiObject.SetActive(true);
        fighterTwoUiObject.SetActive(true);
        gameUiObject.SetActive(true);
        fighterSpeechText.enabled = false;
        fighterOne.ResetFighter();
        fighterTwo.ResetFighter();
        mainCamera.CenterCamera();
        winText.enabled = true;
        winText.text = "FIGHT!";
        time = 99.9f;
        roundedTime = 99;
        timerText.text = "" + roundedTime;
        chargeSpecial = false;
        preFightTimer = 1f;
        preFight = true;
    }

    public void StartFighterZoom()
    {
        fighterOneUiObject.SetActive(false);
        fighterTwoUiObject.SetActive(false);
        gameUiObject.SetActive(false);
        zoomInCamera.EnableCamera();
        mainCamera.DisableCamera();
        zoomInCamera.StartCharacterIntro();
    }

    public void SetIntroQuote(bool isFighterOne)
    {
        fighterSpeechText.enabled = true;
        string fullIntroQuote;
        if (isFighterOne)
        {
            fullIntroQuote = fighterOne.GetRandomIntroQuote();
        }
        else
        {
            fullIntroQuote = fighterTwo.GetRandomIntroQuote();
        }
        StartCoroutine(SpellOutQuote(fullIntroQuote));
    }

    public void SetVictoryQuote(bool isFighterOne)
    {
        fighterSpeechText.enabled = true;
        string fullVictoryQuote;
        if (isFighterOne)
        {
            fullVictoryQuote = fighterOne.GetRandomVictoryQuote();
        }
        else
        {
            fullVictoryQuote = fighterTwo.GetRandomVictoryQuote();
        }
        StartCoroutine(SpellOutQuote(fullVictoryQuote));
    }

    private IEnumerator SpellOutQuote(string quote)
    {
        for (int i = 0; i < quote.Length+1; i++)
        {
            fighterSpeechText.text = quote.Substring(0, i);
            yield return new WaitForSeconds(.02f);
        }
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
            winText.text = "FIGHTER 1 WON THE ROUND!";
           
        }
        else
        {
            fighterTwoWins++;
            fighterTwoUI.UpdateWins(fighterTwoWins);
            winText.text = "FIGHTER 2 WON THE ROUND!";
        }
        if (postRound)
        {
            winText.text = "TIE ROUND";
        }
        postRound = true;
        postRoundTimer = 3f;
    }

    public void DealDamageToFighter(int damage, AttackType attackType, bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            damageToDealToPlayerOne += damage;
            typeOfDamageToDealToPlayerOne = attackType;
        } else
        {
            damageToDealToPlayerTwo += damage;
            typeOfDamageToDealToPlayerTwo = attackType;
        }
    }

    public void DisplayRematchMenu()
    {
        gameUiObject.SetActive(false);
        winText.enabled = false;
        rematchMenuUiObject.SetActive(true);
    }

    private bool DetermineWinner()  //Returns true if a fighter won the match
    { 
        if (fighterOneWins != roundsToWin && fighterTwoWins != roundsToWin)
        {
            return false;
        }
        if (fighterOneWins == roundsToWin && fighterTwoWins == roundsToWin)
        {
            winText.text = "TIE";
            postFight = true;
            postRoundTimer = 3f;
        }
        else if (fighterOneWins == roundsToWin)
        {
            zoomInCamera.StartFighterOneVictory(fighterOne.gameObject.transform.position.x);
            winText.text = "FIGHTER 1 WINS!";
            mainCamera.DisableCamera();
            zoomInCamera.EnableCamera();
        }
        else
        {
            zoomInCamera.StartFighterTwoVictory(fighterTwo.gameObject.transform.position.x);
            winText.text = "FIGHTER 2 WINS!";
            mainCamera.DisableCamera();
            zoomInCamera.EnableCamera();
        }

        return true;
    }

    public bool AbleToMoveForward()
    {
        return fighterOne.GetRightmostPosition() < fighterTwo.GetLeftmostPosition();
    }

    public void PlayZoomInCameraAnimation(string animationName)
    {
        zoomInCamera.PlayAnimation(animationName);
    }

    public bool IsPlayerOnePaused()
    {
        return playerOnePaused;
    }

    public void PauseGame(bool isPlayerOne)
    {
        gameActive = false;
        playerOnePaused = isPlayerOne;
        fighterOneSavedVelocity = fighterOne.GetRigidbody().velocity;
        fighterOne.GetRigidbody().isKinematic = true;
        fighterOne.GetAnimator().enabled = false;
        fighterTwoSavedVelocity = fighterTwo.GetRigidbody().velocity;
        fighterTwo.GetRigidbody().isKinematic = true;
        fighterTwo.GetAnimator().enabled = false;
        pauseMenuObject.SetActive(true);
        string playerText;
        if (isPlayerOne)
        {
            playerText = "P1";
        } else
        {
            playerText = "P2";
        }
        string pauseText = playerText + " Paused";
        pauseMenuObject.GetComponentInChildren<Text>().text = pauseText;
    }

    public void ResumeGame()
    {
        gameActive = true;
        if (playerOnePaused)
        {
            fighterOne.ActivateDelayFrame();
        }
        else
        {
            fighterTwo.ActivateDelayFrame();
        }
        menuNavigation.DeactivateMenuBothPlayers();
        fighterOne.GetRigidbody().isKinematic = false;
        fighterOne.GetRigidbody().velocity = fighterOneSavedVelocity;
        fighterOne.GetAnimator().enabled = true;
        fighterTwo.GetRigidbody().isKinematic = false;
        fighterTwo.GetRigidbody().velocity = fighterTwoSavedVelocity;
        fighterTwo.GetAnimator().enabled = true;
        pauseMenuObject.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToFighterSelection()
    {
        GameData.SetSameSettings(true);
        SceneManager.LoadScene("Main Menu");
    }

    private GameObject DecideFighter(characters character)
    {
        switch (character)
        {
            case characters.WillNeff:
                return willNeffFighter;
            case characters.HasanAbi:
                return hasanAbiFighter;
            case characters.Nesua:
                return nesuaFighter;
            case characters.red:
                return redFighter;
            default:
                return null;
        }
    }

}
