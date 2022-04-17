using UnityEditor;
using UnityEngine;

namespace BlobIO.Blobs.Tentacles
{
    public class Tentacle : MonoBehaviour
    {
        private const float MIN_TENTACLE_LENGTH = 1f;

        [SerializeField] private float _resolution = 0.5f;
        [SerializeField] private LineRenderer _lineRenderer;

        private int _dynamicPointCount;
        private float _length;
        private float _stiffness;
        private float _damp;
        private float _compression;
        private float _verticalOffset;
        private Vector2 _lastOffset;
        private Vector2 _velocity;
        private TentaclePoint _basePoint;
        private TentaclePoint _tipPoint;

        public Vector2 TipPoint => _tipPoint.Position;

        public void Construct(TentaclePoint basePoint, TentaclePoint tipPoint, float stiffness, float damp,
            float verticalOffset)
        {
            float radius = Mathf.Max(Vector2.Distance(basePoint.Position, tipPoint.Position), MIN_TENTACLE_LENGTH);

            Construct(basePoint, tipPoint, stiffness, damp, verticalOffset, radius);
        }

        public void Construct(TentaclePoint basePoint, TentaclePoint tipPoint, float stiffness, float damp,
            float verticalOffset, float radius)
        {
            _verticalOffset = verticalOffset;
            _basePoint = basePoint;
            _tipPoint = tipPoint;
            _stiffness = stiffness;
            _damp = damp;
            _length = radius;

            _dynamicPointCount = 1;

            if (_basePoint.IsDynamic || _tipPoint.IsDynamic)
                _dynamicPointCount++;
        }

        private void Update()
        {
            UpdateRenderer();
        }

        private void FixedUpdate()
        {
            Vector2 force = CalculateForce(_tipPoint.Position, _basePoint.Position - Vector2.up * _verticalOffset) /
                            _dynamicPointCount;
            _basePoint.AddForce(force);
            _tipPoint.AddForce(-force);
        }

        private void UpdateRenderer()
        {
            float currentLength = Vector2.Distance(_basePoint.Position, _tipPoint.Position);
            int pointCount = (int) (currentLength / (1f / _resolution));

            if (pointCount > 1)
            {
                Vector3 basePosition = _basePoint.Position;
                Vector3 tipPosition = _tipPoint.Position;
                Vector3[] points = new Vector3[pointCount];

                for (int i = 0; i < pointCount; i++)
                {
                    float t = Mathf.Clamp01((float) i / (pointCount - 1));
                    points[i] = Vector3.Lerp(basePosition, tipPosition, t);
                }

                _lineRenderer.positionCount = pointCount;
                _lineRenderer.SetPositions(points);
            }
            else
            {
                _lineRenderer.positionCount = 0;
            }
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