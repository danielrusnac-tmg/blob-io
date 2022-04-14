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

        [Tooltip("Starts aligned with the movement direction.")]
        [SerializeField] private AnimationCurve _angleBias = AnimationCurve.Constant(0f, 1f, 1f);

        private bool _hasAController;
        private bool _isMoving;
        private Vector2 _desiredPoint;
        private Vector2 _moveDirection = Vector2.right;
        private IControllableInput _input;
        private Rigidbody2D _rb;
        private RaycastHit2D[] _tentacleHits;

        public void Construct(Rigidbody2D rb)
        {
            _tentacleHits = new RaycastHit2D[1];
            _rb = rb;
        }

        private void Awake()
        {
            Construct(GetComponent<Rigidbody2D>());
        }

        private void Update()
        {
            ReadInput();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
            Handles.DrawLine(transform.position, transform.position + (Vector3) _moveDirection * _radius);

            // Handles.color = Color.red;
            // Handles.DrawSolidDisc(_desiredPoint, Vector3.forward, 0.2f);
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

        private void Move()
        {
            if (!_isMoving)
                return;

            for (int i = 0; i < 10; i++)
            {
                float angle = GetRandomAngle();
                Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * _moveDirection;

                if (Physics2D.RaycastNonAlloc(transform.position, direction, _tentacleHits, _radius, _wallMask) > 0)
                {
                    _desiredPoint = _tentacleHits[0].point;
                    Debug.DrawLine(transform.position, _desiredPoint, new Color(1f, 0.92f, 0.02f, 0.03f), 1f);
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