using UnityEngine;

namespace BlobIO.Services.Factory
{
    public interface IGameFactory : IService
    {
        void Warmup();
        void Cleanup();
        void CreatePlayer(Vector3 position);
    }
}