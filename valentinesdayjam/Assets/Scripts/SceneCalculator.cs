using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SceneCalculator : MonoBehaviour
    {
        private static SceneCalculator _instance;
        private int _level = -1;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(transform.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartNewGame()
        {
            _level = 0;
            ClickAsync("Tutorial");
        }

        public void GoToNextLevel()
        {
            _level++;
            Debug.Log("NEXT LEVEL!!");
        }

        public void GoToLevelSelect()
        {
            Debug.Log("LEVEL SELECT!!");
        }

        public void GoToMainMenu()
        {
            ClickAsync("MainMenu");
        }

        public Slider LoadingBar;
        public GameObject LoadingImage;

        private AsyncOperation _async;

        private void ClickAsync(string name)
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
            LoadingImage.SetActive(false);
        }
    }
}
