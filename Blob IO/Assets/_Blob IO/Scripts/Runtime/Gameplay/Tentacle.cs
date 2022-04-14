using UnityEngine;

namespace BlobIO.Gameplay
{
    public class Tentacle
    {
        private readonly Vector2 _direction;
        private readonly float _length;
        private readonly LayerMask _solidLayer;
        private readonly Spring _spring;
        private readonly RaycastHit2D[] _hits;

        public Tentacle(float angle, float length, LayerMask solidLayer, Spring spring)
        {
            _direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.up;
            _length = length;
            _solidLayer = solidLayer;
            _spring = spring;
            _hits = new RaycastHit2D[1];
        }

        public Vector2 CalculateForce(Vector2 center, float deltaTime)
        {
            if (IsHittingAWall(center))
            {
                Debug.DrawLine(center, _hits[0].point, Color.red, deltaTime);
            }
            else
            {
                Debug.DrawRay(center, _direction * _length, Color.green, deltaTime);
                return Vector2.zero;
            }
            
            Vector2 offset = (center - _hits[0].point).normalized * _length * 0.5f;
            Vector3 force = _spring.CalculateForce(_hits[0].point + offset, center, deltaTime);
            
            return force;
        }

        private bool IsHittingAWall(Vector2 center)
        {
            return Physics2D.RaycastNonAlloc(center, _direction, _hits, _length, _solidLayer) > 0;
        }
    }
}