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

    private MenuNavigation playerTwoNavigation;

    public void Init()
    {
        controllerInput = ControllerManager.GetControllerInput(1);
        lastButtonPressed = ButtonTypes.None;
        repeatDelayCount = 0f;
        menuActive = false;
        lockedMenu = false;
        isPlayerOne = true;
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
    }

    void Update()
    {
        CheckForButtonInput();
        if (isPlayerOne && controllerInput.GetTopActionButtonDown())
        {
            print("menu active: " + menuActive);
            print("lockedMenu: " + lockedMenu);
            print("menu script: " + menuScript);
        }
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
                    DeselectButton();
                    do
                    {
                        currentX--;
                        if (currentX < 0)
                        {
                            currentX = maxX;
                        }
                    } while (!ValidButton() && currentX != startX);
                    lastButtonPressed = ButtonTypes.Left;
                    SelectButton();
                }
                else if (controllerInput.GetXAxisRight()
                    && (repeatDelayCount <= 0f || lastButtonPressed != ButtonTypes.Right))
                {
                    DeselectButton();
                    do
                    {
                        currentX++;
                        if (currentX > maxX)
                        {
                            currentX = 0;
                        }
                    } while (!ValidButton() && currentX != startX);
                    lastButtonPressed = ButtonTypes.Right;
                    SelectButton();
                }
                else if (controllerInput.GetYAxisUp()
                    && (repeatDelayCount <= 0f || lastButtonPressed != ButtonTypes.Up))
                {
                    DeselectButton();
                    do
                    {
                        currentY--;
                        if (currentY < 0)
                        {
                            currentY = maxY;
                        }
                    } while (!ValidButton() && currentY != startY);
                    lastButtonPressed = ButtonTypes.Up;
                    SelectButton();
                }
                else if (controllerInput.GetYAxisDown()
                    && (repeatDelayCount <= 0f || lastButtonPressed != ButtonTypes.Down))
                {
                    DeselectButton();
                    do
                    {
                        currentY++;
                        if (currentY > maxY)
                        {
                            currentY = 0;
                        }
                    } while (!ValidButton() && currentY != startY);
                    lastButtonPressed = ButtonTypes.Down;
                    SelectButton();
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
        if (isPlayerOne)
        {
            playerTwoNavigation.DeactivateMenu();
        }
        SelectButton();
        buttonMap[currentY, currentX].OnSelect(null);
    }

    public void LoadMenu(BaseMenuScript baseMenuScript, Button[,] buttonMap, 
        int defaultButtonX, int defaultButtonY, int defaultButtonXTwo, int defaultButtonYTwo)
    {
        LoadMenu(baseMenuScript, buttonMap, defaultButtonX, defaultButtonY);
        playerTwoNavigation.LoadMenu(baseMenuScript, buttonMap, defaultButtonXTwo, defaultButtonYTwo);
    }

    public void DeactivateMenu()
    {
        menuActive = false;
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

    private bool ValidButton()
    {
        return buttonMap[currentY, currentX] != null;
    }

    public void DeselectButton()
    {
        buttonMap[currentY, currentX].OnDeselect(null);
    }

    private void SelectButton()
    {
        buttonMap[currentY, currentX].Select();
        buttonMap[currentY, currentX].OnSelect(null);
        repeatDelayCount = repeatDelay;
    }

    private void PressButton()
    {
        GameData.SetPressedButtonPlayerOne(isPlayerOne);
        buttonMap[currentY, currentX].onClick.Invoke();
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