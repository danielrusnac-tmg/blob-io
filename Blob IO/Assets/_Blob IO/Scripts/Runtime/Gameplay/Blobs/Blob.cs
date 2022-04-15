using System.Collections;
using System.Collections.Generic;
using BlobIO.Gameplay.Controllers;
using UnityEditor;
using UnityEngine;

namespace BlobIO.Gameplay.Blobs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Blob : MonoBehaviour, IControllable
    {
        private const int MAX_TRY_POINT = 3;
        
        private bool _hasAController;
        private bool _isMoving;
        private Vector2 _moveDirection = Vector2.right;
        private RaycastHit2D[] _tentacleHits;
        private Rigidbody2D _rb;
        private GlobalSettings _globalSettings;
        private BlobSettings _blobSettings;
        private IControllableInput _input;
        private List<Tentacle> _activeTentacles = new List<Tentacle>();

        public void Construct(GlobalSettings globalSettings, BlobSettings blobSettings)
        {
            _globalSettings = globalSettings;
            _blobSettings = blobSettings;
            _activeTentacles = new List<Tentacle>();

            StartCoroutine(UpdateTentaclesRoutine());
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _tentacleHits = new RaycastHit2D[1];
        }

        private void Update()
        {
            ReadInput();
        }

        private void FixedUpdate()
        {
            ApplyTentacleForces();
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            float radius = _blobSettings ? _blobSettings.Radius : 8f;
            
            Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
            Handles.DrawLine(transform.position, transform.position + (Vector3) _moveDirection * radius);
            
           foreach (Tentacle tentacle in _activeTentacles)
               tentacle.DrawGizmos();
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

        private void ApplyTentacleForces()
        {
            Vector2 force = Vector2.zero;

            foreach (Tentacle activeTentacle in _activeTentacles)
            {
                force += activeTentacle.CalculateForce(Time.fixedDeltaTime);
            }

            _rb.AddForce(force * _rb.mass);
        }

        private IEnumerator UpdateTentaclesRoutine()
        {
            while (true)
            {
                if (_isMoving)
                {
                    RemoveUnwantedTentacle();
                    UpdateTentacleCount();
                }

                yield return new WaitForSeconds(_blobSettings.UpdateTentaclesDelay);
            }
        }

        private void UpdateTentacleCount()
        {
            if (_activeTentacles.Count < _blobSettings.WantedTentacleCount)
            {
                CreateTentacle();
            }
        }

        private void CreateTentacle()
        {
            if (!TryGetRandomDesiredPoint(out Vector2 point, out float distance))
                return;
            
            Tentacle tentacle = new Tentacle(_blobSettings.CreateSpring(distance), transform, point);
            _activeTentacles.Add(tentacle);
        }

        private void RemoveUnwantedTentacle()
        {
            float minDesirability = float.MaxValue;
            Tentacle unwantedTentacle = null;
            
            foreach (Tentacle tentacle in _activeTentacles)
            {
                float desirability = CalculateDesirability(tentacle.Point);

                if (desirability < minDesirability)
                {
                    minDesirability = desirability;
                    unwantedTentacle = tentacle;
                }
            }
            
            if (unwantedTentacle == null || minDesirability > _blobSettings.RemoveDesireThreshold)
                return;

            _activeTentacles.Remove(unwantedTentacle);
        }

        private bool TryGetRandomDesiredPoint(out Vector2 point, out float radius)
        {
            for (int i = 0; i < MAX_TRY_POINT; i++)
            {
                float angle = _blobSettings.GetRandomAngleOffset();
                Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * _moveDirection;

                if (Physics2D.RaycastNonAlloc(transform.position, direction, _tentacleHits, _blobSettings.Radius, _globalSettings.WallMask) > 0)
                {
                    radius = _tentacleHits[0].distance; 
                    point = _tentacleHits[0].point;
                    return true;
                }
            }

            radius = 0f;
            point = Vector2.zero;
            return false;
        }

        private float CalculateDesirability(Vector2 point)
        {
            Vector2 offset = point - (Vector2) transform.position;
            float distance = -offset.magnitude / _blobSettings.Radius;
            float angle = Vector2.Dot(offset.normalized, _moveDirection);

            return distance + angle;
        }
    }
}