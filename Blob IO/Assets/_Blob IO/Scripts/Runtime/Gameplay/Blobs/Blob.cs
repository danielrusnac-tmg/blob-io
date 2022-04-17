using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.Blobs
{
    public class Blob : MonoBehaviour, IControllable
    {
        [SerializeField] private int _currentTentacleCount;
        [SerializeField] private int _maxTentacleCount = 10;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private Rigidbody2D _blobRigidbody;

        private IControllableInput _input;
        
        public void SetInput(IControllableInput input)
        {
            _input = input;
        }

        private void FixedUpdate()
        {
            float t = GetActiveTentaclesPercent();
            Vector2 movement = (_input.IsMoving ? _input.MoveDirection : Vector2.zero) * _speed;
            _blobRigidbody.AddForce((movement * t) - _blobRigidbody.velocity, ForceMode2D.Impulse);
            _blobRigidbody.AddForce(Physics2D.gravity * (1f - t), ForceMode2D.Impulse);
        }

        private float GetActiveTentaclesPercent()
        {
            return (float)_currentTentacleCount / _maxTentacleCount;
        }
    }
}