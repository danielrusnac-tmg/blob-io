using BlobIO.Gameplay.Controllers;
using UnityEditor;
using UnityEngine;

namespace BlobIO.Gameplay.Blobs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Blob : MonoBehaviour, IControllable
    {
        [SerializeField] private LayerMask _wallMask;
        [SerializeField] private float _radius = 8f;
        [SerializeField] private float _desireTreshold;
        [SerializeField] private Spring _spring;

        [Tooltip("Starts aligned with the movement direction.")]
        [SerializeField] private AnimationCurve _angleBias = AnimationCurve.Constant(0f, 1f, 1f);

        private bool _hasAController;
        private bool _isMoving;
        private Vector2 _desiredPoint;
        private Vector2 _moveDirection = Vector2.right;
        private IControllableInput _input;
        private Rigidbody2D _rb;
        private RaycastHit2D[] _tentacleHits;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _tentacleHits = new RaycastHit2D[1];
        }

        private void Update()
        {
            ReadInput();
            UpdateTentacles();
        }

        private void FixedUpdate()
        {
            Vector2 force = _spring.CalculateForce(_desiredPoint, transform.position, Time.fixedDeltaTime);
            _rb.AddForce(force * _rb.mass);
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
            Handles.DrawLine(transform.position, transform.position + (Vector3) _moveDirection * _radius);
            
            Handles.color = Color.red;
            Handles.DrawSolidDisc(_desiredPoint, Vector3.forward, 0.2f);
#endif
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
            _hasAController = input != null;
        }

        private void ReadInput()
        {
            if (!_hasAController)
                return;

            if (_input.MoveDirection.sqrMagnitude < Constants.EPSILON)
            {
                _isMoving = false;
                return;
            }

            _moveDirection = _input.MoveDirection.normalized;
            _isMoving = true;
        }

        private void UpdateTentacles()
        {
            if (!_isMoving)
                return;

            for (int i = 0; i < 10; i++)
            {
                float angle = GetRandomAngle();
                Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * _moveDirection;

                if (Physics2D.RaycastNonAlloc(transform.position, direction, _tentacleHits, _radius, _wallMask) > 0)
                {
                    float currentDesirability = EvaluateDesirability(_desiredPoint);
                    float wantedDesirability = EvaluateDesirability(_tentacleHits[0].point);
                    
                    if (wantedDesirability - currentDesirability > _desireTreshold)
                        _desiredPoint = _tentacleHits[0].point;
                    
                    break;
                }
            }
        }

        private float GetRandomAngle()
        {
            return _angleBias.Evaluate(Random.value) * 180f;
        }

        private float EvaluateDesirability(Vector2 point)
        {
            Vector2 offset = point - (Vector2) transform.position;
            float distance = -offset.magnitude / _radius;
            float angle = Vector2.Dot(offset.normalized, _moveDirection);

            return distance + angle;
        }
    }
}