using System.Collections.Generic;
using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class BetterBlob : MonoBehaviour, IControllable
    {
        private IControllableInput _input;
        
        public void SetInput(IControllableInput input)
        {
            _input = input;
        }
    }
}