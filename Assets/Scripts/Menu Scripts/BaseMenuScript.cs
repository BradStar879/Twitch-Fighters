using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseMenuScript : MonoBehaviour
{
    protected Button[,] buttonMap;
    protected MenuNavigation menuNavigation;
    [SerializeField] protected Button defaultButton;
    private int defaultButtonX;
    private int defaultButtonY;

    private void Awake()
    {
        menuNavigation = GameObject.FindWithTag("Game Manager").GetComponent<MenuNavigation>();
        Init();
        for (int y = 0; y < buttonMap.GetLength(0); y++)
        {
            for (int x = 0; x < buttonMap.GetLength(1); x++)
            {
                if (buttonMap[y, x] == defaultButton)
                {
                    defaultButtonX = x;
                    defaultButtonY = y;
                }
            }
        }
    }

    private void OnEnable()
    {
        LoadMenu();
        ResetMenu();
    }

    public void LoadMenu()
    {
        menuNavigation.LoadMenu(this, buttonMap, defaultButtonX, defaultButtonY);
    }

    public virtual void Cancel(bool isPlayerOne) {}

    protected virtual void ResetMenu() {}

    protected abstract void Init();
}
