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
        private const int LAST_LEVEL_INDEX = 9;

        public Dictionary<int, string> LevelNumberToName = new Dictionary<int, string>();
        private static SceneCalculator _instance;
        private int _level = -1;

        public bool HasBeatenGame { get; private set; } = false;

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

        private void Start()
        {
            LevelNumberToName.Add(0, "Tutorial");
            LevelNumberToName.Add(1, "Level1");
            LevelNumberToName.Add(2, "Level2");
            LevelNumberToName.Add(3, "Level3");
            LevelNumberToName.Add(4, "Level4");
            LevelNumberToName.Add(5, "Level5");
            LevelNumberToName.Add(6, "Level6");
            LevelNumberToName.Add(7, "Level7");
            LevelNumberToName.Add(8, "Level8");
            LevelNumberToName.Add(9, "Level9");
        }

        public void StartNewGame()
        {
            GoToLevelNumber(0);
        }

        public void JustBeatLevel()
        {
            if (_level == LAST_LEVEL_INDEX)
                HasBeatenGame = true;
        }

        public void GoToNextLevel()
        {
            GoToLevelNumber(_level + 1);
        }

        public void GoToLevelNumber(int index)
        {
            if (LevelNumberToName.ContainsKey(index))
            {
                _level = index;
                ClickAsync(LevelNumberToName[index]);
            }
            else
            {
                Debug.Log("HEY NO LEVEL : " + index);
            }
        }

        public void GoToLevelSelect()
        {
            ClickAsync("LevelSelect");
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
