using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameScreen : MonoBehaviour
{
    private SceneCalculator _sceneCalculator;

    // Start is called before the first frame update
    void Start()
    {
        _sceneCalculator = FindObjectOfType<SceneCalculator>();
    }

    public void GoToNextLevel()
    {
        _sceneCalculator.GoToNextLevel();
    }

    public void GoToLevelSelect()
    {
        _sceneCalculator.GoToLevelSelect();
    }

    public void GoToMainMenu()
    {
        _sceneCalculator.GoToMainMenu();
    }
}
