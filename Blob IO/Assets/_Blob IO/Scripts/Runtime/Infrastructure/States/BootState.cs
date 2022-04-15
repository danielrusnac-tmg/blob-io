using BlobIO.Services;
using BlobIO.Services.AssetManagement;
using BlobIO.Services.Factory;
using BlobIO.Services.Input;

namespace BlobIO.Infrastructure.States
{
    public class BootState : IGameState
    {
        private readonly GameStateMachine _stateMachine;

        public BootState(AllServices services, GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            RegisterServices(services);
        }

        private static void RegisterServices(AllServices services)
        {
            services.RegisterSingle<IInputService>(new SimpleInputService());
            services.RegisterSingle<IAssetProvider>(new ResourcesAssetProvider());
            RegisterGameFactory(services);
        }

        public void Enter()
        {
            _stateMachine.Enter<InitializeLevelState>();
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