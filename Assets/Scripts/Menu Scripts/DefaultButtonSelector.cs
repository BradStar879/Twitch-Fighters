using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultButtonSelector : MonoBehaviour
{

    [SerializeField] Button defaultSelectedButton;

    private void OnEnable()
    {
        SelectDefaultButton();
    }

    public void SelectDefaultButton()
    {
        defaultSelectedButton.Select();
        defaultSelectedButton.OnSelect(null);
    }
}
