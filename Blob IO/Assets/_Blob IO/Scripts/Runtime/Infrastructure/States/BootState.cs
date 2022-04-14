using BlobIO.Services;
using BlobIO.Services.AssetManagement;
using BlobIO.Services.Factory;
using BlobIO.Services.Input;

namespace BlobIO.Infrastructure.States
{
    public class BootState : IGameState
    {
        public BootState(AllServices services)
        {
            RegisterServices(services);
        }

        private static void RegisterServices(AllServices services)
        {
            services.RegisterSingle<IInputService>(new SimpleInputService());
            services.RegisterSingle<IAssetProvider>(new AssetProvider());
            services.RegisterSingle<IGameFactory>(new GameFactory(services.Single<IAssetProvider>()));
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}