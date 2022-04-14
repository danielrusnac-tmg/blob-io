using UnityEngine;

namespace BlobIO.Gameplay.Blobs.Controllers
{
    public interface IControllableInput
    {
        Vector2 Movement { get; }
    }
}