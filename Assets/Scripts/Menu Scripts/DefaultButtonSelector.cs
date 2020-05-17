using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultButtonSelector : MonoBehaviour
{

    private EventSystem eventSystem;
    [SerializeField] GameObject defaultSelectedButton;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        SelectDefaultButton();
    }

    public void SelectDefaultButton()
    {
        eventSystem.SetSelectedGameObject(defaultSelectedButton);
    }
}
