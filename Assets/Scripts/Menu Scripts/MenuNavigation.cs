using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] bool multiplePlayersSelecting = false;
    [SerializeField] float repeatDelay = .2f;
    private ControllerInput playerOneControllerInput;
    private ControllerInput playerTwoControllerInput;
    private Button[,] buttonMap;
    private int currentX;
    private int currentY;
    private int maxX;
    private int maxY;
    private ButtonTypes lastButtonPressed;
    private float repeatDelayCount;

    public void Init()
    {
        playerOneControllerInput = ControllerManager.GetControllerInput(1);
        playerTwoControllerInput = ControllerManager.GetControllerInput(2);
        lastButtonPressed = ButtonTypes.None;
        repeatDelayCount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (repeatDelayCount > 0f)
        {
            repeatDelayCount -= Time.deltaTime;
        }
        int startX = currentX;
        int startY = currentY;
        if (playerOneControllerInput.GetXAxisLeft() && 
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
        else if (playerOneControllerInput.GetXAxisRight()
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
        else if (playerOneControllerInput.GetYAxisUp() 
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
        else if (playerOneControllerInput.GetYAxisDown() 
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
        else if (playerOneControllerInput.GetBottomActionButtonDown())
        {
            PressButton();
        }
    }

    public void LoadMenu(Button[,] buttonMap, int defaultButtonX, int defaultButtonY)
    {
        this.buttonMap = buttonMap;
        this.currentX = defaultButtonX;
        this.currentY = defaultButtonY;
        this.maxX = buttonMap.GetLength(1) - 1;
        this.maxY = buttonMap.GetLength(0) - 1;
        SelectButton();
        buttonMap[currentY, currentX].OnSelect(null);
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