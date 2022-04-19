using System;
using BlobIOEditor.Constants;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlobIOEditor.ColdStart
{
    public class ColdStartBoostrap
    {
        private ColdStartData _data;
        private readonly string _booScenePath;
        private Scene _bootScene;

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
            _data = EditorPrefs.HasKey(EditorSaveKeys.COLD_START_DATA) 
                ? JsonUtility.FromJson<ColdStartData>(EditorPrefs.GetString(EditorSaveKeys.COLD_START_DATA)) 
                : new ColdStartData();
        }

        private void Save()
        {
            EditorPrefs.SetString(EditorSaveKeys.COLD_START_DATA, JsonUtility.ToJson(_data));
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    LogColdStart();
                    break;
                    
                case PlayModeStateChange.EnteredEditMode:
                    OpenLastOpenedScenes();
                    CloseBoot();
                    break;
                
                case PlayModeStateChange.ExitingEditMode:
                    RememberOpenedScenes();

                    if (IsBootSceneLoaded())
                    {
                        _data.WasColdStarted = false;
                        return;
                    }
                    
                    _data.WasColdStarted = true;
                    OpenBoot();
                    Save();
                    break;
            }
        }

        private void LogColdStart()
        {
            if (_data.WasColdStarted)
                Debug.Log("Cold Start");
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
                EditorSceneManager.CloseScene(_bootScene, true);
        }

        private void RememberOpenedScenes()
        {
            _data.LastOpenedScenesPaths = new string[EditorSceneManager.loadedSceneCount];
            
            for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
                _data.LastOpenedScenesPaths[i] = SceneManager.GetSceneAt(i).path;
        }

        private void OpenLastOpenedScenes()
        {
            foreach (string scenePath in _data.LastOpenedScenesPaths)
                EditorSceneManager.OpenScene(scenePath);
            
            _data.LastOpenedScenesPaths = Array.Empty<string>();
        }
    }
}