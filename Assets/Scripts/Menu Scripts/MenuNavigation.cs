using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    private BaseMenuScript menuScript;
    private float repeatDelay = .2f;
    private ControllerInput controllerInput;
    private Button[,] buttonMap;
    private int currentX;
    private int currentY;
    private int maxX;
    private int maxY;
    private ButtonTypes lastButtonPressed;
    private float repeatDelayCount;
    private bool menuActive;
    private bool lockedMenu;
    private bool isPlayerOne;

    private GameObject playerSelector;
    private Texture unselectedSprite;
    private Texture selectedSprite;
    private GameObject cpuSelector;
    private bool selectedSpriteActive;
    private bool cpuSelectorActive;
    private GameObject canvas;

    private MenuNavigation playerTwoNavigation;

    public void Init()
    {
        controllerInput = ControllerManager.GetControllerInput(1);
        lastButtonPressed = ButtonTypes.None;
        repeatDelayCount = 0f;
        menuActive = false;
        lockedMenu = false;
        isPlayerOne = true;
        selectedSpriteActive = false;
        cpuSelectorActive = false;
        canvas = GameObject.FindWithTag("Canvas");
        playerSelector = Instantiate(Resources.Load<GameObject>("Prefabs/UI/P1 Selector"), canvas.transform);
        playerSelector.SetActive(false);
        cpuSelector = Instantiate(Resources.Load<GameObject>("Prefabs/UI/CPU Selector"), canvas.transform);
        cpuSelector.SetActive(false);
        unselectedSprite = Resources.Load<Texture>("2D Images/Player 1");
        selectedSprite = Resources.Load<Texture>("2D Images/Player 1 Selected");
        playerTwoNavigation = GetComponents<MenuNavigation>()[1];
        playerTwoNavigation.SecondPlayerInit();
    }

    public void SecondPlayerInit()
    {
        controllerInput = ControllerManager.GetControllerInput(2);
        lastButtonPressed = ButtonTypes.None;
        repeatDelayCount = 0f;
        menuActive = false;
        lockedMenu = false;
        isPlayerOne = false;
        selectedSpriteActive = false;
        cpuSelectorActive = false;
        canvas = GameObject.FindWithTag("Canvas");
        playerSelector = Instantiate(Resources.Load<GameObject>("Prefabs/UI/P2 Selector"), canvas.transform);
        playerSelector.SetActive(false);
        cpuSelector = Instantiate(Resources.Load<GameObject>("Prefabs/UI/CPU Selector"), canvas.transform);
        cpuSelector.SetActive(false);
        unselectedSprite = Resources.Load<Texture>("2D Images/Player 2");
        selectedSprite = Resources.Load<Texture>("2D Images/Player 2 Selected");
    }

    void Update()
    {
        CheckForButtonInput();
    }

    public void CheckForButtonInput()
    {
        if (menuActive)
        {
            if (!lockedMenu)
            {
                if (repeatDelayCount > 0f)
                {
                    repeatDelayCount -= Time.deltaTime;
                }
                int startX = currentX;
                int startY = currentY;
                if (controllerInput.GetXAxisLeft() &&
                    (repeatDelayCount <= 0f || lastButtonPressed != ButtonTypes.Left))
                {
                    do
                    {
                        currentX--;
                        if (currentX < 0)
                        {
                            currentX = maxX;
                        }
                    } while (!ValidButton() && currentX != startX);
                    lastButtonPressed = ButtonTypes.Left;
                    if (currentX != startX)
                    {
                        DeselectButton(startX, startY);
                        SelectButton();
                    }
                }
                else if (controllerInput.GetXAxisRight()
                    && (repeatDelayCount <= 0f || lastButtonPressed != ButtonTypes.Right))
                {
                    do
                    {
                        currentX++;
                        if (currentX > maxX)
                        {
                            currentX = 0;
                        }
                    } while (!ValidButton() && currentX != startX);
                    lastButtonPressed = ButtonTypes.Right;
                    if (currentX != startX)
                    {
                        DeselectButton(startX, startY);
                        SelectButton();
                    }
                }
                else if (controllerInput.GetYAxisUp()
                    && (repeatDelayCount <= 0f || lastButtonPressed != ButtonTypes.Up))
                {
                    do
                    {
                        currentY--;
                        if (currentY < 0)
                        {
                            currentY = maxY;
                        }
                    } while (!ValidButton() && currentY != startY);
                    lastButtonPressed = ButtonTypes.Up;
                    if (currentY != startY)
                    {
                        DeselectButton(startX, startY);
                        SelectButton();
                    }
                }
                else if (controllerInput.GetYAxisDown()
                    && (repeatDelayCount <= 0f || lastButtonPressed != ButtonTypes.Down))
                {
                    do
                    {
                        currentY++;
                        if (currentY > maxY)
                        {
                            currentY = 0;
                        }
                    } while (!ValidButton() && currentY != startY);
                    lastButtonPressed = ButtonTypes.Down;
                    if (currentY != startY)
                    {
                        DeselectButton(startX, startY);
                        SelectButton();
                    }
                }
                else if (controllerInput.GetBottomActionButtonDown())
                {
                    PressButton();
                }
            }
            if (controllerInput.GetRightActionButtonDown())
            {
                menuScript.Cancel(isPlayerOne);
            }
        }
    }

    public void LoadMenu(BaseMenuScript baseMenuScript, Button[,] buttonMap, 
        int defaultButtonX, int defaultButtonY)
    {
        this.menuScript = baseMenuScript;
        this.buttonMap = buttonMap;
        this.currentX = defaultButtonX;
        this.currentY = defaultButtonY;
        this.maxX = buttonMap.GetLength(1) - 1;
        this.maxY = buttonMap.GetLength(0) - 1;
        menuActive = true;
        lockedMenu = false;
        playerSelector.SetActive(true);
        selectedSpriteActive = false;
        playerSelector.GetComponent<RawImage>().texture = unselectedSprite;
        if (isPlayerOne)
        {
            playerTwoNavigation.DeactivateMenu();
        }
        SelectButton();
    }

    public void LoadMenu(BaseMenuScript baseMenuScript, Button[,] buttonMap, 
        int defaultButtonX, int defaultButtonY, int defaultButtonXTwo, int defaultButtonYTwo)
    {
        LoadMenu(baseMenuScript, buttonMap, defaultButtonX, defaultButtonY);
        playerTwoNavigation.LoadMenu(baseMenuScript, buttonMap, defaultButtonXTwo, defaultButtonYTwo);
    }

    public void DeactivateMenu()
    {
        playerSelector.SetActive(false);
        cpuSelector.SetActive(false);
        menuActive = false;
    }

    public void DeactivateMenuBothPlayers()
    {
        DeactivateMenu();
        playerTwoNavigation.DeactivateMenu();
    }

    public void LockPlayerOneSelection()
    {
        lockedMenu = true;
    }

    public void LockPlayerTwoSelection()
    {
        playerTwoNavigation.LockPlayerOneSelection();
    }

    public void UnlockPlayerOneSelection()
    {
        lockedMenu = false;
    }

    public void UnlockPlayerTwoSelection()
    {
        playerTwoNavigation.UnlockPlayerOneSelection();
    }

    public void SwapSelector()
    {
        GameObject tempSelector = playerSelector;
        playerSelector = cpuSelector;
        cpuSelector = tempSelector;
        playerSelector.SetActive(true);
        playerSelector.transform.position = cpuSelector.transform.position;
        cpuSelector.SetActive(false);
        cpuSelectorActive = !cpuSelectorActive;
        SelectButton();
    }

    private bool ValidButton()
    {
        return buttonMap[currentY, currentX] != null;
    }

    public bool IsButtonSelected(Button button)
    {
        return button == buttonMap[currentY, currentX];
    }

    public void DeselectButton(int x, int y)
    {
        buttonMap[y, x].OnDeselect(null);
    }

    private void SelectButton()
    {
        Rect buttonRect = ((RectTransform) buttonMap[currentY, currentX].transform).rect;
        Vector2 buttonPosition = buttonMap[currentY, currentX].transform.position;

        float canvasScale = canvas.transform.localScale.x;
        float playerSelectorX;
        float playerSelectorY = buttonPosition.y + (buttonRect.height * 3/4 * canvasScale);
        if (isPlayerOne && !cpuSelectorActive)
        {
            playerSelectorX = buttonPosition.x - (buttonRect.width / 2 * canvasScale);
        }
        else
        {
            playerSelectorX = buttonPosition.x + (buttonRect.width / 2 * canvasScale);
        }
        playerSelector.transform.position = new Vector2(playerSelectorX, playerSelectorY);
        repeatDelayCount = repeatDelay;
    }

    private void PressButton()
    {
        GameData.SetPressedButtonPlayerOne(isPlayerOne);
        buttonMap[currentY, currentX].onClick.Invoke();
    }

    public void SwapSelectedSprite()
    {
        selectedSpriteActive = !selectedSpriteActive;
        if (selectedSpriteActive)
        {
            playerSelector.GetComponent<RawImage>().texture = selectedSprite;
        }
        else
        {
            playerSelector.GetComponent<RawImage>().texture = unselectedSprite;
        }
    }

    public void SwapSelectedSpritePlayerTwo()
    {
        playerTwoNavigation.SwapSelectedSprite();
    }

    public int GetX()
    {
        return currentX;
    }

    public int GetPlayerTwoX()
    {
        return playerTwoNavigation.GetX();
    }
}

public enum ButtonTypes
{
    None,
    Left,
    Right,
    Down,
    Up
}