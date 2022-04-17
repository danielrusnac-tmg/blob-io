using System;
using UnityEngine;

namespace BlobIO.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; }
        event Action RestartButtonPressed;
    }
}