﻿using System;
using BlobIO.Constants;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlobIO
{
    [Serializable]
    public struct ColdStartData
    {
        public string[] LastOpenedScenesPaths;

        public ColdStartData(string[] lastOpenedScenesPaths)
        {
            LastOpenedScenesPaths = lastOpenedScenesPaths;
        }
    }
    
    public class ColdStartBoostrap 
    {
        private readonly string _booScenePath;
        private Scene _bootScene;
        private string[] _lastOpenedScenesPaths;

        public ColdStartBoostrap(string booScenePath)
        {
            _booScenePath = booScenePath;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            Load();
        }

        ~ColdStartBoostrap()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void Load()
        {
            _lastOpenedScenesPaths = JsonUtility.FromJson<ColdStartData>(EditorPrefs.GetString(EditorSaveKeys.LAST_OPENED_SCENES)).LastOpenedScenesPaths;
        }

        private void Save()
        {
            EditorPrefs.SetString(EditorSaveKeys.LAST_OPENED_SCENES, JsonUtility.ToJson(new ColdStartData(_lastOpenedScenesPaths)));
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OpenLastOpenedScenes();
                    CloseBoot();
                    break;
                
                case PlayModeStateChange.ExitingEditMode:
                    RememberOpenedScenes();
                    
                    if (IsBootSceneLoaded())
                        return;
                    
                    OpenBoot();
                    break;
            }
        }

        private bool IsBootSceneLoaded()
        {
            return SceneManager.GetActiveScene().path == _booScenePath;
        }

        private void OpenBoot()
        {
            _bootScene = EditorSceneManager.OpenScene(_booScenePath, OpenSceneMode.Single);
        }

        private void CloseBoot()
        {
            if (_bootScene.isLoaded)
            {
                EditorSceneManager.CloseScene(_bootScene, true);
            }
        }

        private void RememberOpenedScenes()
        {
            _lastOpenedScenesPaths = new string[EditorSceneManager.loadedSceneCount];
            
            for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
                _lastOpenedScenesPaths[i] = SceneManager.GetSceneAt(i).path;
            
            Save();
        }

        private void OpenLastOpenedScenes()
        {
            foreach (string scenePath in _lastOpenedScenesPaths)
            {
                EditorSceneManager.OpenScene(scenePath);
            }
            
            _lastOpenedScenesPaths = Array.Empty<string>();
        }
    }
}