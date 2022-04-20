using System;
using UnityEngine;

namespace BlobIO.SoftBody
{
    [Serializable]
    public class SoftBodySpringSetting
    {
        [SerializeField] private float _frequency = 10f;
        [SerializeField] private float _damping = 0.6f;
        public float Frequency => _frequency;
        public float Damping => _damping;
    }
}