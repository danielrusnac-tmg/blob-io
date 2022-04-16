using UnityEngine;

namespace BlobIO.Controllers
{
    public interface IControllableInput
    {
        Vector2 MoveDirection { get; }
    }
}