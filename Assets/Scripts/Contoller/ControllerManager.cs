using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerManager : MonoBehaviour
{
    private int maxControllers = 2;
    private static ControllerInput[] controllerInputs;
    private static List<int> controllerSpotsToIgnore;
    private static Dictionary<int, int> connectedControllerMap;    //Key - Controller spot, Value - Player Number
    [SerializeField] EventSystem eventSystem;
    private StandaloneInputModule inputModule; //Used for menu navigation

    public void Start()
    {
        Init();
        inputModule = eventSystem.GetComponent<StandaloneInputModule>();
        //Cursor.lockState = CursorLockMode.locked;
        //Cursor.visible = false;
    }

    public void Init()
    {
        if (controllerInputs == null)
        {
            controllerInputs = new ControllerInput[maxControllers];
            for (int i = 0; i < maxControllers; i++)
            {
                controllerInputs[i] = new ControllerInput();
                controllerInputs[i].Init(i + 1);
            }
        }

        if (controllerSpotsToIgnore == null)
        {
            controllerSpotsToIgnore = new List<int>();
            for (int i = 0; i < Input.GetJoystickNames().Length; i++)   //Ignore all blank connections
            {
                if (Input.GetJoystickNames()[i].Length == 0)
                {
                    controllerSpotsToIgnore.Add(i);
                }
            }
        }

        if (connectedControllerMap == null)
        {
            connectedControllerMap = new Dictionary<int, int>();
        }
    }

    void FixedUpdate()
    {
        string[] joystickNames = Input.GetJoystickNames();
        List<int> disconnectedControllerSpots = new List<int>();
        foreach (KeyValuePair<int, int> connectedController in connectedControllerMap)
        {
            int controllerSpot = connectedController.Key;
            int connectedPlayerNumber = connectedController.Value;
            if (joystickNames[controllerSpot].Length == 0)  //Controller has been disconnected
            {
                controllerInputs[connectedPlayerNumber].ActivateKeyboardInput();
                disconnectedControllerSpots.Add(controllerSpot);
            }
        }
        foreach (int controllerSpot in disconnectedControllerSpots)
        {
            connectedControllerMap.Remove(controllerSpot);

        }

        int playerNumber = 0;   //Note: Starts at 0, not 1
        for (int i = 0; i < joystickNames.Length; i++)
        {
            if (!controllerSpotsToIgnore.Contains(i))
            {
                if (joystickNames[i].Length > 0)
                {
                    if (!controllerInputs[playerNumber].IsUsingController())
                    {
                        controllerInputs[playerNumber].ActivateControllerInput(i + 1);
                        connectedControllerMap.Add(i, playerNumber);
                    }
                    playerNumber++;
                }
            }

            if (playerNumber >= maxControllers)
            {
                break;
            }
        }

        if (inputModule != null)
        {
            ControllerInput playerOneControllerInput = GetControllerInput(1);
            inputModule.horizontalAxis = playerOneControllerInput.GetXAxisString();
            inputModule.verticalAxis = playerOneControllerInput.GetYAxisString();
            inputModule.submitButton = playerOneControllerInput.GetBottomActionButtonString();
            inputModule.cancelButton = playerOneControllerInput.GetRightActionButtonString();
        }
    }

    public static ControllerInput GetControllerInput(int playerNumber)  //Player numbers start at 1
    {
        return controllerInputs[playerNumber - 1];
    }

}
