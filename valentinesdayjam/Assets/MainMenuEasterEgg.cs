using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEasterEgg : MonoBehaviour
{
    private SceneCalculator _sceneCalculator;

    private void Start()
    {
        _sceneCalculator = FindObjectOfType<SceneCalculator>();
        if (_sceneCalculator.HasBeatenGame)
            GetComponent<SpriteRenderer>().enabled = true;
    }
}
