using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.Blobs
{
    public class BetterBlob : MonoBehaviour, IControllable
    {
        [SerializeField] private int _tentacleCount = 10;

        private IControllableInput _input;

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }
    }
}