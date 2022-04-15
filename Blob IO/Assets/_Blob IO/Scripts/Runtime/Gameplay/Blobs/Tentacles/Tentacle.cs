using UnityEditor;
using UnityEngine;

namespace BlobIO.Gameplay.Blobs.Tentacles
{
    public class Tentacle : MonoBehaviour
    {
        private float _length;
        private float _stiffness;
        private float _damp;
        private float _compression;
        private int _dynamicPointCount;
        private Vector2 _lastOffset;
        private Vector2 _velocity;
        private TentaclePoint _basePoint;
        private TentaclePoint _tipPoint;

        public Vector2 TipPoint => _tipPoint.Position;

        public void Construct(TentaclePoint basePoint, TentaclePoint tipPoint, float stiffness, float damp)
        {
            _basePoint = basePoint;
            _tipPoint = tipPoint;
            _stiffness = stiffness;
            _damp = damp;
            _length = Mathf.Max(Vector2.Distance(_basePoint.Position, _tipPoint.Position), Constants.MIN_TENTACLE_LENGTH);

            _dynamicPointCount = 1;
            
            if (_basePoint.IsDynamic || _tipPoint.IsDynamic)
                _dynamicPointCount++;
        }

        private void FixedUpdate()
        {
            Vector2 force = CalculateForce(_tipPoint.Position, _basePoint.Position) / _dynamicPointCount;
            _basePoint.AddForce(force);
            _tipPoint.AddForce(-force);
        }
        
        private Vector2 CalculateForce(Vector2 staticPoint, Vector2 dynamicPoint)
        {
            Vector2 offset = dynamicPoint - staticPoint;
            _velocity = (offset - _lastOffset) / Time.fixedDeltaTime;
            _lastOffset = offset;

            _compression = offset.magnitude / _length;
            Vector2 force = _compression * -_stiffness * offset.normalized;
            force -= _damp * _velocity;

            return force;
        }
        
        public void DrawGizmos()
        {
#if UNITY_EDITOR
            if (_basePoint == null || _tipPoint == null)
                return;
            
            Handles.color = Color.green;
            Handles.DrawSolidDisc(_basePoint.Position, Vector3.forward, 0.1f);

            Handles.color = Color.Lerp(Color.green, Color.red, _compression);
            Handles.DrawLine(_basePoint.Position, _tipPoint.Position);
            Handles.DrawSolidDisc(_tipPoint.Position, Vector3.forward, 0.1f);
#endif
        }
    }
}