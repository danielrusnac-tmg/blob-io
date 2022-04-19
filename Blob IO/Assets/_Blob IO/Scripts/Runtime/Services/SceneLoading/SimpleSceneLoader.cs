using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlobIO.Services.SceneLoading
{
    public class SimpleSceneLoader : ISceneLoader
    {
        private int[] _levels = new[] { 1, 2 };
        
        public void LoadLevel(int levelIndex, Action onLoaded)
        {
            SceneManager.LoadScene(GetValidLevelIndex(levelIndex));
            onLoaded?.Invoke();
        }

        private int GetValidLevelIndex(int levelIndex)
        {
            return _levels[(int)Mathf.Repeat(levelIndex, _levels.Length)];
        }
    }
}