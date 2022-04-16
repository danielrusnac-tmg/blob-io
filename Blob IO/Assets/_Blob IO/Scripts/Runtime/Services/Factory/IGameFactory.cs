using UnityEngine;

namespace BlobIO.Services.Factory
{
    public interface IGameFactory : IService
    {
        void Cleanup();
        void CreatePlayer(Vector3 position);
    }
}