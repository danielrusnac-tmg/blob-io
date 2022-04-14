namespace BlobIO.Infrastructure.States
{
    public interface IGameState
    {
        void Enter();
        void Exit();
    }
}