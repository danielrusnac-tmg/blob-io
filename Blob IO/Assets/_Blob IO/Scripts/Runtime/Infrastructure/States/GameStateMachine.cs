using System;
using System.Collections.Generic;
using BlobIO.Services;
using BlobIO.Services.Factory;

namespace BlobIO.Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IGameState> _stateByType;
        private IGameState _currentState;

        public GameStateMachine(AllServices services)
        {
            _stateByType = new Dictionary<Type, IGameState>
            {
                {typeof(BootState), new BootState(services, this)},
                {typeof(InitializeLevelState), new InitializeLevelState(services.Single<IGameFactory>())}
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