using System.Collections;
using UnityEditor;
using UnityEngine;

namespace BlobIO.Blobs.Tentacles
{
    public class Tentacle : MonoBehaviour
    {
        private const float MIN_TENTACLE_LENGTH = 1f;
        
        [SerializeField] private float _resolution = 0.5f;
        [SerializeField] private LineRenderer _lineRenderer;
        
        [Header("Animation")]
        [SerializeField] private float _grabDuration = 0.3f;
        [SerializeField] private float _releaseDuration = 0.3f;
        [SerializeField] private float _wobbleAmount;
        [SerializeField] private float _wobbleFrequency;
        [Range(0f, 1f)]
        [SerializeField] private float _renderPercent;
        [SerializeField] private AnimationCurve _grabEase;
        [SerializeField] private AnimationCurve _releaseEase;
        [SerializeField] private AnimationCurve _wobbleEase;
        
        private int _dynamicPointCount;
        private float _wobbleOffset;
        private float _length;
        private float _stiffness;
        private float _damp;
        private float _compression;
        private float _verticalOffset;
        private Vector2 _lastOffset;
        private Vector2 _velocity;
        private TentaclePoint _basePoint;
        private TentaclePoint _tipPoint;

        public bool CanApplyForce { get; private set; }
        public float Weight { get; set; } = 1f;
        public Vector2 TipPoint => _tipPoint.Position;

        public void Construct(TentaclePoint basePoint, TentaclePoint tipPoint, float stiffness, float damp,
            float verticalOffset)
        {
            _wobbleOffset = Random.value;
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

            StartCoroutine(nameof(SpawnAnimationRoutine));
        }

        private void Update()
        {
            UpdateRenderer();
        }

        private void FixedUpdate()
        {
            if (!CanApplyForce)
                return;
            
            Vector2 force = CalculateForce(_tipPoint.Position, _basePoint.Position - Vector2.up * _verticalOffset) /
                            _dynamicPointCount * Weight;
            _basePoint.AddForce(force);
            _tipPoint.AddForce(-force);
        }

        public void Despawn()
        {
            StopCoroutine(nameof(SpawnAnimationRoutine));
            StartCoroutine(nameof(DespawnAnimationRoutine));
        }

        private IEnumerator SpawnAnimationRoutine()
        {
            CanApplyForce = false;
            float time = 0f;

            while (time < _grabDuration)
            {
                time += Time.deltaTime;
                float t = time / _grabDuration;

                // _wobbleAmount = Mathf.Lerp(0.7f, 0f, _grabEase.Evaluate(t));
                // _wobbleFrequency = Mathf.Lerp(-10f, 0f, _grabEase.Evaluate(t));
                _renderPercent = Mathf.Lerp(0f, 1f, _grabEase.Evaluate(t));
                
                yield return null;
            }

            // _wobbleAmount = 0f;
            // _wobbleFrequency = 0f;
            _renderPercent = 1f;
                
            CanApplyForce = true;
        }

        private IEnumerator DespawnAnimationRoutine()
        {
            CanApplyForce = false;
            float time = 0f;

            while (time < _releaseDuration)
            {
                time += Time.deltaTime;
                float t = time / _releaseDuration;
                
                // _wobbleAmount = Mathf.Lerp(0f, 0.5f, _releaseEase.Evaluate(t));
                // _wobbleFrequency = Mathf.Lerp(0f, -10f, _releaseEase.Evaluate(t));
                _renderPercent = Mathf.Lerp(1f, 0f, _releaseEase.Evaluate(t));
                
                yield return null;
            }
            
            Destroy(gameObject);
        }
        
        private void UpdateRenderer()
        {
            float currentLength = Vector2.Distance(_basePoint.Position, _tipPoint.Position);
            int pointCount = (int) (currentLength / (1f / _resolution));
            
            if (CanApplyForce)
            {
                _wobbleAmount = Mathf.Lerp(0.4f, 0f, _compression);
            }

            if (pointCount > 1)
            {
                Vector3 basePosition = _basePoint.Position;
                Vector3 tipPosition = _tipPoint.Position;
                Vector3[] points = new Vector3[pointCount];

                for (int i = 0; i < pointCount; i++)
                {
                    float t = Mathf.Clamp01((float) i / (pointCount - 1)) * _renderPercent;
                    float wobbleAmount = Mathf.Sin(_wobbleFrequency * Time.time + ((t + _wobbleOffset) * Mathf.PI * 2)) * _wobbleAmount * _wobbleEase.Evaluate(t);
                    Vector2 wobbleAxis = Vector3.Cross((tipPosition - basePosition).normalized, Vector3.forward);

                    points[i] = Vector3.Lerp(basePosition, tipPosition, t);
                    points[i] += (Vector3)wobbleAxis * wobbleAmount;
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