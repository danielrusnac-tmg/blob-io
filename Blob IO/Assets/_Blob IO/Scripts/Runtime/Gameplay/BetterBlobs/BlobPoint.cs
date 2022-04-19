using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class BlobPoint
    {
        public Vector3 Position;
        public Vector3 Normal;
        
        private Vector3 _velocity;
        private Vector3 _force;

        public Vector3 Velocity => _velocity;

        public BlobPoint(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }

        public void CalculateVelocity(float deltaTime)
        {
            _velocity += _force * deltaTime;
        }

        public void UpdatePosition(float deltaTime)
        {
            Position += _velocity * deltaTime;
        }

        public void ResetForce()
        {
            _force = Vector3.zero;
        }

        public void AddForce(Vector3 force)
        {
            _force += force;
        }
    }
}