using System;
using System.Collections.Generic;
using BlobIO.Services;

namespace BlobIO.Infrastructure.States
{
    public class GameStateMachine
    {
        private Dictionary<Type, IGameState> _stateByType;
        private IGameState _currentState;

        public GameStateMachine(AllServices allServices)
        {
            _stateByType = new Dictionary<Type, IGameState>
            {
                {typeof(BootState), new BootState(allServices)},
                {typeof(GameLoopState), new GameLoopState()}
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