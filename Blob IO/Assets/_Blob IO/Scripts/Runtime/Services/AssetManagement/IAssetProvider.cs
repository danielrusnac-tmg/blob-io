using UnityEngine;

namespace BlobIO.Services.AssetManagement
{
    public interface IAssetProvider : IService
    {
        GameObject Load(string path);
    }
}