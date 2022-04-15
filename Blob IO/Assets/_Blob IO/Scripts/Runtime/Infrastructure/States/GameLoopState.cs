using System.Collections;
using BlobIO.Services.Factory;
using BlobIO.Services.Input;
using UnityEngine;

namespace BlobIO.Infrastructure.States
{
    public class GameLoopState : IGameState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IInputService _inputService;

        public GameLoopState(GameStateMachine stateMachine, IGameFactory gameFactory, IInputService inputService)
        {
            _stateMachine = stateMachine;
            _gameFactory = gameFactory;
            _inputService = inputService;
        }
        
        public void Enter()
        {
            _gameFactory.Warmup();
            _gameFactory.CreatePlayer(Vector3.zero);
            _inputService.RestartButtonPressed += OnRestart;
        }

        public void Exit()
        {
            _inputService.RestartButtonPressed -= OnRestart;
            _gameFactory.Cleanup();
        }

        private void OnRestart()
        {
            _stateMachine.Enter<GameLoopState>();
        }
    }
}