using System.Threading.Tasks;
using UnityEngine;

namespace BlobIO.Services.AssetManagement
{
    public interface IAssetProvider : IService
    {
        Task<T> Load<T>(string path) where T : Object;
        Task<GameObject> Load(string path);
    }
}