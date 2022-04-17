using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.SmartBlobs
{
    public class SmartBlob : MonoBehaviour, IControllable
    {
        [SerializeField] private Transform _tentacleContainer;

        private Rigidbody2D _rb;
        private IControllableInput _input;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_input == null)
                return;
            
            _tentacleContainer.up = _input.MoveDirection;
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }
    }
}