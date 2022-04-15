using UnityEngine;

namespace BlobIO.Gameplay.Blobs.Tentacles
{
    public class TentaclePoint
    {
        private Vector2 _forceOffset;
        private Transform _transform;
        private Rigidbody2D _rigidbody;

        public Vector2 Position => (Vector2)_transform.position + (Vector2)_transform.TransformDirection(_forceOffset);
        public bool IsDynamic { get; private set; }
        
        public TentaclePoint(GameObject target, Vector2 point)
        {
            _forceOffset = point - (Vector2)target.transform.position;
            _transform = target.transform;
            _rigidbody = target.GetComponent<Rigidbody2D>();
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