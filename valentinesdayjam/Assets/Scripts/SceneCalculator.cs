﻿using System;
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
        public Dictionary<int, string> LevelNumberToName = new Dictionary<int, string>();
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

        private void Start()
        {
            LevelNumberToName.Add(0, "Tutorial");
            LevelNumberToName.Add(1, "neillevel2");
            LevelNumberToName.Add(2, "Level1");
            LevelNumberToName.Add(3, "neillevel1");
            LevelNumberToName.Add(4, "neillevel3");
        }

        public void StartNewGame()
        {
            _level = 0;
            GoToLevelNumber(_level);
        }

        public void GoToLevelNumber(int index)
        {
            if (LevelNumberToName.ContainsKey(index))
                ClickAsync(LevelNumberToName[index]);
            else
                Debug.Log("HEY NO LEVEL : " + index);
        }

        public void GoToNextLevel()
        {
            _level++;
            GoToLevelNumber(_level);
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
