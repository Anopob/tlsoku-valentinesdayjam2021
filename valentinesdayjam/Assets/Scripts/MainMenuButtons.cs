using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    private SceneCalculator _sceneCalculator;

    private void Start()
    {
        _sceneCalculator = FindObjectOfType<SceneCalculator>();
    }

    public void StartNewGame()
    {
        _sceneCalculator.StartNewGame();
    }

    public void GoToLevelSelect()
    {
        _sceneCalculator.GoToLevelSelect();
    }
}
