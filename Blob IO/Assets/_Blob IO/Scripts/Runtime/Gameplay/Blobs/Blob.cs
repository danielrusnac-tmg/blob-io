using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.Blobs
{
    public class Blob : MonoBehaviour, IControllable
    {
        [Range(0f, 1f)]
        [SerializeField] private float _t;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private Rigidbody2D _blobRigidbody;

        private IControllableInput _input;
        
        public void SetInput(IControllableInput input)
        {
            _input = input;
        }

        private void FixedUpdate()
        {
            Vector2 wantedVelocity = GetWantedVelocity();
            _blobRigidbody.AddForce(wantedVelocity - _blobRigidbody.velocity, ForceMode2D.Impulse);
        }

        private Vector2 GetWantedVelocity()
        {
            Vector2 movement = (_input.IsMoving ? _input.MoveDirection : Vector2.zero) * _speed;
            Vector2 gravity = Physics2D.gravity;
            
            return Vector2.Lerp(gravity, movement, _t);
        }
    }
}