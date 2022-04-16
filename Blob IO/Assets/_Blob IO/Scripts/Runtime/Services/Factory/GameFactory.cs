using System;
using System.Threading.Tasks;
using BlobIO.Blobs;
using BlobIO.Cameras;
using BlobIO.Controllers;
using BlobIO.Services.AssetManagement;
using BlobIO.Services.Input;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public void Warmup()
        {
            
        }

        public async void CreatePlayer(Vector3 position)
        {
            GlobalSettings globalSettings = await _assetProvider.Load<GlobalSettings>(AssetPaths.GLOBAL_SETTINGS);
            BlobSettings blobSettings = await _assetProvider.Load<BlobSettings>(AssetPaths.PLAYER_BLOB_SETTINGS);
            GameObject playerPrefab = await _assetProvider.Load(AssetPaths.PLAYER);
           
            _player = Object.Instantiate(playerPrefab, position, Quaternion.identity);
            _player.GetComponent<Blob>().Construct(globalSettings, blobSettings);
            _player.GetComponent<IControllable>().SetInput(new PlayerInput(_inputService));

            CreateCamera(_player.transform);
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