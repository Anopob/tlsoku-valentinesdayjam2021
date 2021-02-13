using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    private SceneCalculator _sceneCalculator;

    private void Start()
    {
        _sceneCalculator = FindObjectOfType<SceneCalculator>();
    }

    public void GoToLevelNumber(int index)
    {
        _sceneCalculator.GoToLevelNumber(index);
    }

    public void GoToMainMenu()
    {
        _sceneCalculator.GoToMainMenu();
    }
}
