using System;
using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.SmartBlobs
{
    public class SmartBlob : MonoBehaviour, IControllable
    {
        [SerializeField] private SmartTentacle[] _tentacles;

        private IControllableInput _input;

        private void Update()
        {
            if (_input == null || _input.MoveDirection.sqrMagnitude < Constants.EPSILON)
                return;

            float angle = Vector2.SignedAngle(Vector2.up, _input.MoveDirection);
            foreach (SmartTentacle tentacle in _tentacles)
                tentacle.GrabAngle = angle;
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }
    }
}