using UnityEngine;

namespace BlobIO.Controllers
{
    public interface IControllableInput
    {
        bool IsMoving { get; }
        Vector2 MoveDirection { get; }
    }
}