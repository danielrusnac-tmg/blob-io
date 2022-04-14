using UnityEngine;

namespace BlobIO.Services.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreatePlayer(Vector3 position);
    }
}