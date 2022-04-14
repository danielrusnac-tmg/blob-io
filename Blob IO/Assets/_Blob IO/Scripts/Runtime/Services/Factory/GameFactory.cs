using BlobIO.Gameplay.Blobs.Controllers;
using BlobIO.Gameplay.Cameras;
using BlobIO.Services.AssetManagement;
using BlobIO.Services.Input;
using UnityEngine;

namespace BlobIO.Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private GameObject _player;
        private GameplayCamera _gameplayCamera;
        private readonly IAssetProvider _assetProvider;
        private readonly IInputService _inputService;

        public GameFactory(IAssetProvider assetProvider, IInputService inputService)
        {
            _assetProvider = assetProvider;
            _inputService = inputService;
        }

        public void CreatePlayer(Vector3 position)
        {
            GameObject playerPrefab = _assetProvider.Load(AssetPaths.PLAYER);
            _player = Object.Instantiate(playerPrefab, position, Quaternion.identity);
            _player.GetComponent<IControllable>().SetInput(new PlayerInput(_inputService));
            
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