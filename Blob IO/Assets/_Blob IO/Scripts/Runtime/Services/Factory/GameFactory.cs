using BlobIO.Cameras;
using BlobIO.Controllers;
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
        
        public async void CreatePlayer(Vector3 position)
        {
            GameObject playerPrefab = await _assetProvider.Load(AssetPaths.JOINTS_BLOB);
            _player = Object.Instantiate(playerPrefab, position, Quaternion.identity);

            SetPlayerInput();
            CreateCamera(_player.transform);
        }

        private void SetPlayerInput()
        {
            if (_player.TryGetComponent(out IControllable controllable))
            {
                controllable.SetInput(new PlayerInput(_inputService));
                return;
            }
            
            Debug.Log("No Controllable component on player!", _player);
        }

        public void Cleanup()
        {
            Object.Destroy(_gameplayCamera.gameObject);
            Object.Destroy(_player);
        }

        private async void CreateCamera(Transform player)
        {
            GameObject cameraPrefab = await _assetProvider.Load(AssetPaths.CAMERA);
            _gameplayCamera = Object.Instantiate(cameraPrefab).GetComponent<GameplayCamera>();
            _gameplayCamera.SetTarget(player);
        }
    }
}