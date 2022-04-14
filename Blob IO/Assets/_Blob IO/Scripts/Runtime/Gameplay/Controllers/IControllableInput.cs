using UnityEngine;

namespace BlobIO.Gameplay.Controllers
{
    public interface IControllableInput
    {
        Vector2 MoveDirection { get; }
    }
}