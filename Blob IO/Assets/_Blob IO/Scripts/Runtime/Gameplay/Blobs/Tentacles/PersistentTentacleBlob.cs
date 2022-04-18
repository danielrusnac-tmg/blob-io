using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.Blobs.Tentacles
{
    public class PersistentTentacleBlob : MonoBehaviour, IControllable
    {
        [SerializeField] private TentacleBrain _tentacleBrain;
        [SerializeField] private Rigidbody2D _rigidbody;

        private IControllableInput _input;

        private void Update()
        {
            if (_input.IsMoving)
            {
                _tentacleBrain.Look(_input.MoveDirection);
            }
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }
    }
}