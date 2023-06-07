using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game.Utils;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField][Scene] private string _MenuScene;

        private string MenuScene { get => _MenuScene; }

        protected override void Awake()
        {
            base.Awake();
            LoadMenu();
        }

        [ContextMenu("Open Directory")]
        public void OpenDirectory()
        {
            Application.OpenURL(Application.persistentDataPath);
        }

        private void LoadMenu()
        {
#if !UNITY_EDITOR
            SceneManager.LoadScene(MenuScene);
#endif
        }
    }
}
