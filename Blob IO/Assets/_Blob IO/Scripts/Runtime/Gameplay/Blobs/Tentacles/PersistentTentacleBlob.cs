using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.Blobs.Tentacles
{
    public class PersistentTentacleBlob : MonoBehaviour, IControllable
    {
        [SerializeField] private float _inputSpeed = 10f;
        [SerializeField] private TentacleBrain _tentacleBrain;
        [SerializeField] private Rigidbody2D _rigidbody;

        private IControllableInput _input;

        public void Construct(IControllableInput input)
        {
            SetInput(input);
        }

        private void Update()
        {
            _tentacleBrain.RemoveExtraTentacles();
            
            if (_input.IsMoving)
            {
                _tentacleBrain.Look(_input.MoveDirection);
                _tentacleBrain.UpdateTentaclePositions();
                _tentacleBrain.GenerateTentacles();
                _tentacleBrain.UpdateWeights();
                _tentacleBrain.CountGrabbingTentacles();
            }
            else
            {
                _tentacleBrain.ResetWeights();
            }
        }

        private void FixedUpdate()
        {
            if (_input.IsMoving)
            {
                _rigidbody.AddForce((GetInputForce() - _rigidbody.velocity) * _rigidbody.mass);
            }
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }
        
        private Vector2 GetInputForce()
        {
            return (_input.IsMoving ? _input.MoveDirection : Vector2.zero) * _inputSpeed * _tentacleBrain.ActiveTentaclePercent;
        }
    }
}