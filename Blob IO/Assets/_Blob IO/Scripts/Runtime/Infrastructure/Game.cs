using BlobIO.Infrastructure.States;
using BlobIO.Services;

namespace BlobIO.Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _stateMachine;

        public Game(AllServices services)
        {
            _stateMachine = new GameStateMachine(services);
        }

        public void Run()
        {
            _stateMachine.Enter<BootState>();
        }
    }
}