using BlobIO.Services;
using BlobIO.Services.Input;

namespace BlobIO.Infrastructure.States
{
    public class BootState : IGameState
    {
        public BootState(AllServices allServices)
        {
            RegisterServices(allServices);
        }

        private static void RegisterServices(AllServices allServices)
        {
            allServices.RegisterSingle<IInputService>(new SimpleInputService());
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}