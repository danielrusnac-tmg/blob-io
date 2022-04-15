using BlobIO.Infrastructure.States;
using BlobIO.Services;

namespace BlobIO.Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _stateMachine;

        public Game(AllServices services, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = new GameStateMachine(services, coroutineRunner);
        }

        public void Run()
        {
            _stateMachine.Enter<BootState>();
        }
    }
}