using UnityEngine;

namespace BlobIO.Services.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Load(string path)
        {
            return Resources.Load<GameObject>(path);
        }
    }
}