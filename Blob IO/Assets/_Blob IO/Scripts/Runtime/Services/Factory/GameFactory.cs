using BlobIO.Gameplay.Cameras;
using BlobIO.Services.AssetManagement;
using UnityEngine;

namespace BlobIO.Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private GameObject _player;
        private GameplayCamera _gameplayCamera;
        private readonly IAssetProvider _assetProvider;

        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void CreatePlayer(Vector3 position)
        {
            GameObject playerPrefab = _assetProvider.Load(AssetPaths.PLAYER);
            _player = Object.Instantiate(playerPrefab, position, Quaternion.identity);
            
            CreateCamera(_player.transform);
        }

        public void Cleanup()
        {
            Object.Destroy(_gameplayCamera);
            Object.Destroy(_player);
        }

        private void CreateCamera(Transform player)
        {
            GameObject cameraPrefab = _assetProvider.Load(AssetPaths.CAMERA);
            _gameplayCamera = Object.Instantiate(cameraPrefab).GetComponent<GameplayCamera>();
            _gameplayCamera.SetTarget(player);
        }
    }
}