using System;
using UnityEngine;

namespace BlobIO.SoftBody
{
    [Serializable]
    public class SoftBodySpringSetting
    {
        [SerializeField] private float _stiffness = 10f;
        [SerializeField] private float _damping = 0.6f;
        
        public float Stiffness => _stiffness;
        public float Damping => _damping;
    }
}