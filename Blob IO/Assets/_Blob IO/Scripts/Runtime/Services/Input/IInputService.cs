using UnityEngine;

namespace BlobIO.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; }
    }
}