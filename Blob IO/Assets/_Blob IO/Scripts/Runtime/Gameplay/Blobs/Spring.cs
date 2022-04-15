using System;
using UnityEngine;

namespace BlobIO.Gameplay
{
    [Serializable]
    public class Spring : ICloneable
    {
        public float Length;
        public float Stiffness;
        public float Damp;

        private Vector2 _lastOffset;
        private Vector2 _velocity;
        
        public float Compression { get; private set; }

        public Spring()
        {
            Length = 2f;
            Stiffness = 500f;
            Damp = 16f;
        }

        public Spring(float length, float stiffness, float damp)
        {
            Length = length;
            Stiffness = stiffness;
            Damp = damp;
        }

        public Spring(Spring parent)
        {
            Length = parent.Length;
            Stiffness = parent.Stiffness;
            Damp = parent.Damp;
        }

        public Vector2 CalculateForce(Vector2 staticPoint, Vector2 dynamicPoint, float deltaTime)
        {
            Vector2 offset = dynamicPoint - staticPoint;
            _velocity = (offset - _lastOffset) / deltaTime;
            _lastOffset = offset;

            Compression = offset.magnitude / Length;
            Vector2 force = Compression * -Stiffness * offset.normalized;
            force -= Damp * _velocity;

            return force;
        }

        public object Clone()
        {
            return new Spring(Length, Stiffness, Damp);
        }
    }
}