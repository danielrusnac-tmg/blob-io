using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class BetterBlob : MonoBehaviour, IControllable
    {
        private IControllableInput _input;

        private void Update()
        {
            
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }
    }
}