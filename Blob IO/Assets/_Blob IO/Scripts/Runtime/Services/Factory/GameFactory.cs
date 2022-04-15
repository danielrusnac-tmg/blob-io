using System.Threading.Tasks;
using BlobIO.Gameplay;
using BlobIO.Gameplay.Blobs;
using BlobIO.Gameplay.Cameras;
using BlobIO.Gameplay.Controllers;
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
            Task<GameObject> playerPrefab = _assetProvider.Load(AssetPaths.PLAYER);
            Task<GlobalSettings> globalSettings = _assetProvider.Load<GlobalSettings>(AssetPaths.GLOBAL_SETTINGS);
            Task<BlobSettings> blobSettings = _assetProvider.Load<BlobSettings>(AssetPaths.PLAYER_BLOB_SETTINGS);

            await Task.WhenAll(playerPrefab, globalSettings, blobSettings);
            
            _player = Object.Instantiate(playerPrefab.Result, position, Quaternion.identity);
            _player.GetComponent<IControllable>().SetInput(new PlayerInput(_inputService));
            _player.GetComponent<Blob>().Construct(globalSettings.Result, blobSettings.Result);

            CreateCamera(_player.transform);
        }

        public void Cleanup()
        {
            Object.Destroy(_gameplayCamera);
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