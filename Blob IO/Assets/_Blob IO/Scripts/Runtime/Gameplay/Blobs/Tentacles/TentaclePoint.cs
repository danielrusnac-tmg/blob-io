using UnityEngine;

namespace BlobIO.Gameplay.Blobs.Tentacles
{
    public class TentaclePoint
    {
        private readonly Vector2 _forceOffset;
        private readonly Transform _transform;
        private readonly Rigidbody2D _rigidbody;

        public bool IsDynamic { get; }
        public float Desirability { get; set; }
        
        public Vector2 Position => (Vector2)_transform.position + (Vector2)_transform.TransformVector(_forceOffset);

        public TentaclePoint(GameObject target, Vector2 point)
        {
            _transform = target.transform;
            _rigidbody = target.GetComponent<Rigidbody2D>();
            _forceOffset = _transform.InverseTransformVector(point - (Vector2)_transform.position);
            IsDynamic = _rigidbody != null;
        }

        public void AddForce(Vector2 force)
        {
            if (!IsDynamic)
                return;

            _rigidbody.AddForceAtPosition(force, Position);
        }
    }
}