using System.Threading.Tasks;
using UnityEngine;

namespace BlobIO.Services.AssetManagement
{
    public class ResourcesAssetProvider : IAssetProvider
    {
        public Task<T> Load<T>(string path) where T : Object
        {
            return Task.FromResult(Resources.Load<T>(path));
        }

        public Task<GameObject> Load(string path)
        {
            return Task.FromResult(Resources.Load<GameObject>(path));
        }
    }
}