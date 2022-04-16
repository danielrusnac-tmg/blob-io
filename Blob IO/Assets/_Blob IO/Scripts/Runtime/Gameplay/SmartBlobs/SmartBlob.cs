using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.SmartBlobs
{
    public class SmartBlob : MonoBehaviour, IControllable
    {
        [SerializeField] private SmartTentacle[] _tentacles;

        private Rigidbody2D _rb;
        private IControllableInput _input;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }
    }
}