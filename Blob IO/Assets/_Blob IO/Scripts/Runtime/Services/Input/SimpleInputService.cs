using System;
using System.Collections;
using BlobIO.Infrastructure;
using UnityEngine;

namespace BlobIO.Services.Input
{
    public class SimpleInputService : IInputService
    {
        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";

        private Coroutine _tickCoroutine;
        private readonly ICoroutineRunner _coroutineRunner;

        public bool IsJoystickPressed => SimpleInput.GetMouseButton(0);
        public Vector2 Axis => new Vector2(SimpleInput.GetAxisRaw(HORIZONTAL), SimpleInput.GetAxisRaw(VERTICAL));
        public event Action RestartButtonPressed;

        public SimpleInputService(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _coroutineRunner.StartCoroutine(TickRoutine());
        }

        ~SimpleInputService()
        {
            _coroutineRunner.StopCoroutine(_tickCoroutine);
        }

        private IEnumerator TickRoutine()
        {
            while (true)
            {
                OnTick();
                yield return null;
            }
        }

        private void OnTick()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.R))
            {
                RestartButtonPressed?.Invoke();
            }
        }
    }
}