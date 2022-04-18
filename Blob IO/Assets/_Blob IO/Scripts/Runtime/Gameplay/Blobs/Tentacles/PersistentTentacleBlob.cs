using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.Blobs.Tentacles
{
    public class PersistentTentacleBlob : MonoBehaviour, IControllable
    {
        [SerializeField] private float _inputSpeed = 10f;
        [SerializeField] private float _tentacleSpeed = 4f;
        [SerializeField] private float _gravitySpeed;
        
        [SerializeField] private TentacleBrain _tentacleBrain;
        [SerializeField] private Rigidbody2D _rigidbody;

        private IControllableInput _input;

        public void Construct(IControllableInput input)
        {
            SetInput(input);
        }

        private void Update()
        {
            if (_input.IsMoving)
            {
                _tentacleBrain.Look(_input.MoveDirection);
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(GetInputForce(), ForceMode2D.Impulse);
            _rigidbody.AddForce(GetGravityForce(), ForceMode2D.Impulse);
            _rigidbody.AddForce(GetTentaclesForce(), ForceMode2D.Impulse);
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }
        
        private Vector2 GetInputForce()
        {
            return (_input.IsMoving ? _input.MoveDirection : Vector2.zero) * _inputSpeed * _tentacleBrain.ActiveTentaclePercent - _rigidbody.velocity;
        }

        private Vector2 GetGravityForce()
        {
            return -Vector2.up * _gravitySpeed * (1f - _tentacleBrain.ActiveTentaclePercent);
        }

        private Vector2 GetTentaclesForce()
        {
            return (_tentacleBrain.MidPoint - _rigidbody.position).normalized * _tentacleSpeed * _tentacleBrain.AverageTentacleStretchiness * _tentacleBrain.ActiveTentaclePercent - _rigidbody.velocity;
        }
    }
}