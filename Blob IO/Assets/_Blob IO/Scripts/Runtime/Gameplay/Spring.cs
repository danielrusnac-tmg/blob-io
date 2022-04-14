using System;
using UnityEngine;

namespace BlobIO.Gameplay
{
    [Serializable]
    public class Spring
    {
        [SerializeField] private float _length;
        [SerializeField] private float _stiffness;
        [SerializeField] private float _damp;

        private Vector2 _lastOffset;
        private Vector2 _velocity;

        public Spring()
        {
            _length = 2f;
            _stiffness = 500f;
            _damp = 16f;
        }

        public Spring(float length, float stiffness, float damp)
        {
            _length = length;
            _stiffness = stiffness;
            _damp = damp;
        }

        public Vector2 CalculateForce(Vector2 staticPoint, Vector2 dynamicPoint)
        {
            Vector2 offset = dynamicPoint - staticPoint;
            _velocity = (offset - _lastOffset) / Time.fixedDeltaTime;
            _lastOffset = offset;
            
            Vector2 force = offset.magnitude / _length * -_stiffness * offset.normalized;
            force -= _damp * _velocity;

            return force;
        }
    }
}