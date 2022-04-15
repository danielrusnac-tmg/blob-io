using BlobIO.Services;
using BlobIO.Services.AssetManagement;
using BlobIO.Services.Factory;
using BlobIO.Services.Input;

namespace BlobIO.Infrastructure.States
{
    public class BootState : IGameState
    {
        private readonly GameStateMachine _stateMachine;

        public BootState(GameStateMachine stateMachine, AllServices services, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            RegisterServices(services, coroutineRunner);
        }

        private static void RegisterServices(AllServices services, ICoroutineRunner coroutineRunner)
        {
            services.RegisterSingle<ICoroutineRunner>(coroutineRunner);
            services.RegisterSingle<IInputService>(new SimpleInputService(coroutineRunner));
            services.RegisterSingle<IAssetProvider>(new ResourcesAssetProvider());
            RegisterGameFactory(services);
        }

        public void Enter()
        {
            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }

        private static void RegisterGameFactory(AllServices services)
        {
            services.RegisterSingle<IGameFactory>(new GameFactory(
                    services.Single<IAssetProvider>(), 
                    services.Single<IInputService>()));
        }
    }
}