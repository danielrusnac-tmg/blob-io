using System.Collections;
using System.Threading.Tasks;
using BlobIO.Services.Factory;
using BlobIO.Services.Input;
using BlobIO.Services.SceneLoading;
using UnityEngine;

namespace BlobIO.Infrastructure.States
{
    public class GameLoopState : IGameState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IInputService _inputService;
        private readonly ISceneLoader _sceneLoader;
        private int _levelIndex;

        public GameLoopState(GameStateMachine stateMachine, IGameFactory gameFactory, IInputService inputService, ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _gameFactory = gameFactory;
            _inputService = inputService;
            _sceneLoader = sceneLoader;
        }
        
        public void Enter()
        {
            _sceneLoader.LoadLevel(_levelIndex, OnLevelLoaded);
            _levelIndex++;
        }

        public void Exit()
        {
            _inputService.RestartButtonPressed -= OnRestart;
            _gameFactory.Cleanup();
        }

        private void OnLevelLoaded()
        {
            _gameFactory.CreatePlayer(Vector3.up * 2);
            _inputService.RestartButtonPressed += OnRestart;
        }

        private void OnRestart()
        {
            _stateMachine.Enter<GameLoopState>();
        }
    }
}