using System;
using System.Collections.Generic;
using BlobIO.Services;
using BlobIO.Services.Factory;
using BlobIO.Services.Input;

namespace BlobIO.Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IGameState> _stateByType;
        private IGameState _currentState;

        public GameStateMachine(AllServices services, ICoroutineRunner coroutineRunner)
        {
            _stateByType = new Dictionary<Type, IGameState>
            {
                {typeof(BootState), new BootState(this, services, coroutineRunner)},
                {typeof(GameLoopState), new GameLoopState(this, services.Single<IGameFactory>(), services.Single<IInputService>())}
            };
        }

        public void Enter<T>() where T : IGameState
        {
            _currentState?.Exit();
            _currentState = _stateByType[typeof(T)];
            _currentState.Enter();
        }
    }
}