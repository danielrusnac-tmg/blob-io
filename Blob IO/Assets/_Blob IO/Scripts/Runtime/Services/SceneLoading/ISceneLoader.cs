using System;

namespace BlobIO.Services.SceneLoading
{
    public interface ISceneLoader : IService
    {
        void LoadLevel(int levelIndex, Action onLoaded);
    }
}