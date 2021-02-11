using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickToLoadAsync : MonoBehaviour
{
    public Slider LoadingBar;
    public GameObject LoadingImage;

    private AsyncOperation _async;

    public void ClickAsync(string name)
    {
        LoadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(name));
    }

    private IEnumerator LoadLevelWithBar(string name)
    {
        _async = SceneManager.LoadSceneAsync(name);
        while (!_async.isDone)
        {
            LoadingBar.value = _async.progress;
            yield return null;
        }
    }
}
