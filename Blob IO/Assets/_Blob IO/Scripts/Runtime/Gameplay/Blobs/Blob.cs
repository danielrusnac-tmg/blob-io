using BlobIO.Gameplay.Controllers;
using UnityEditor;
using UnityEngine;

namespace BlobIO.Gameplay.Blobs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Blob : MonoBehaviour, IControllable
    {
        private GlobalSettings _globalSettings;
        private BlobSettings _blobSettings;
        private bool _hasAController;
        private bool _isMoving;
        private Vector2 _desiredPoint;
        private Vector2 _moveDirection = Vector2.right;
        private IControllableInput _input;
        private Rigidbody2D _rb;
        private RaycastHit2D[] _tentacleHits;

        public void Construct(GlobalSettings globalSettings, BlobSettings blobSettings)
        {
            _globalSettings = globalSettings;
            _blobSettings = blobSettings;
        }

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
            // Vector2 force = _spring.CalculateForce(_desiredPoint, transform.position, Time.fixedDeltaTime);
            // _rb.AddForce(force * _rb.mass);
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            float radius = _blobSettings ? _blobSettings.Radius : 8f;
            
            Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
            Handles.DrawLine(transform.position, transform.position + (Vector3) _moveDirection * radius);
            
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
                float angle = _blobSettings.GetRandomAngleOffset();
                Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * _moveDirection;

                if (Physics2D.RaycastNonAlloc(transform.position, direction, _tentacleHits, _blobSettings.Radius, _globalSettings.WallMask) > 0)
                {
                    float currentDesirability = EvaluateDesirability(_desiredPoint);
                    float wantedDesirability = EvaluateDesirability(_tentacleHits[0].point);
                    
                    if (wantedDesirability - currentDesirability > _blobSettings.DesireThreshold)
                        _desiredPoint = _tentacleHits[0].point;
                    
                    break;
                }
            }
        }

        private float EvaluateDesirability(Vector2 point)
        {
            Vector2 offset = point - (Vector2) transform.position;
            float distance = -offset.magnitude / _blobSettings.Radius;
            float angle = Vector2.Dot(offset.normalized, _moveDirection);

            return distance + angle;
        }
    }
}