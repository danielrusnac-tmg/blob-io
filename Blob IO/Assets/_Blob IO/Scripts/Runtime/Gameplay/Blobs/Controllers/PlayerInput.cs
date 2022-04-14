using BlobIO.Services.Input;
using UnityEngine;

namespace BlobIO.Gameplay.Blobs.Controllers
{
    public class PlayerInput : IControllableInput
    {
        private readonly IInputService _inputService;

        public PlayerInput(IInputService inputService)
        {
            _inputService = inputService;
        }

        public Vector2 Movement => _inputService.Axis;
    }
}