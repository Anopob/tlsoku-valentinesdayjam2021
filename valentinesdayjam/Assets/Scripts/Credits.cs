using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Credits : MonoBehaviour
{
    [SerializeField]
    private GameObject _creditsPanel, _mainPanel;

    public void TurnOnCredits()
    {
        _creditsPanel.SetActive(true);
        _mainPanel.SetActive(false);
    }

    public void TurnOffCredits()
    {
        _creditsPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }
}
