using BlobIO.Services.Factory;
using UnityEngine;

namespace BlobIO.Infrastructure.States
{
    public class InitializeLevelState : IGameState
    {
        private readonly IGameFactory _gameFactory;

        public InitializeLevelState(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }
        
        public void Enter()
        {
            _gameFactory.CreatePlayer(Vector3.zero);
        }

        public void Exit()
        {
        }
    }
}