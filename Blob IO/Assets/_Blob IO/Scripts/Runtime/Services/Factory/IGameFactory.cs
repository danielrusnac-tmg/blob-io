using UnityEngine;

namespace BlobIO.Services.Factory
{
    public interface IGameFactory : IService
    {
        void CreatePlayer(Vector3 position);
        void Cleanup();
    }
}