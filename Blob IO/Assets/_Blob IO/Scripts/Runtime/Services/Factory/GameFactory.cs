using BlobIO.Services.AssetManagement;
using UnityEngine;

namespace BlobIO.Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;

        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public GameObject CreatePlayer(Vector3 position)
        {
            GameObject playerPrefab = _assetProvider.Load(AssetPaths.PLAYER);
            return Object.Instantiate(playerPrefab, position, Quaternion.identity);
        }
    }
}