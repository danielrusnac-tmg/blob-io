using System.Collections;
using System.Collections.Generic;
using BlobIO.Blobs.Tentacles;
using BlobIO.Controllers;
using UnityEditor;
using UnityEngine;

namespace BlobIO.Blobs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Blob : MonoBehaviour, IControllable
    {
        private const int MAX_TRY_POINT = 10;

        private static readonly RaycastHit2D[] s_tentacleHits;
        
        [SerializeField] private LayerMask _wallMask;
        [SerializeField] private Tentacle _tentaclePrefab;
        [SerializeField] private BlobSettings _settings;

        private bool _hasAController;
        private bool _isMoving;
        private Vector2 _moveDirection = Vector2.right;
        private IControllableInput _input;
        private List<Tentacle> _activeTentacles = new List<Tentacle>();

        static Blob()
        {
            s_tentacleHits = new RaycastHit2D[1];
        }

        private void Start()
        {
            _activeTentacles = new List<Tentacle>();

            StartCoroutine(UpdateTentaclesRoutine());
        }

        private void Update()
        {
            ReadInput();
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            float radius = _settings.Radius;

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

        private IEnumerator UpdateTentaclesRoutine()
        {
            while (true)
            {
                if (_isMoving)
                {
                    RemoveUnwantedTentacle();
                    UpdateTentacleCount();
                }

                yield return new WaitForSeconds(_settings.UpdateTentaclesDelay);
            }
        }

        private void UpdateTentacleCount()
        {
            for (int i = 0; i < MAX_TRY_POINT; i++)
            {
                float angle = _settings.GetRandomAngleOffset();
                Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * _moveDirection;
                Vector2 origin = (Vector2) transform.position + direction * _settings.BlobRadius;

                if (Physics2D.RaycastNonAlloc(origin, direction, s_tentacleHits, _settings.Radius, _wallMask) > 0)
                {
                    if (_activeTentacles.Count < _settings.WantedTentacleCount)
                    {
                        _activeTentacles.Add(CreateTentacle(origin));
                    }
                    else
                    {
                        float newPointDesirability = CalculateDesirability(s_tentacleHits[0].point);
                        float minDesirability = GetMinTentacleDesirability(out Tentacle unwantedTentacle);

                        if (minDesirability - newPointDesirability < _settings.DesireThreshold)
                        {
                            RemoveTentacle(unwantedTentacle);
                            _activeTentacles.Add(CreateTentacle(origin));
                            break;
                        }
                    }
                }
            }
        }

        private void RemoveTentacle(Tentacle unwantedTentacle)
        {
            _activeTentacles.Remove(unwantedTentacle);
            Destroy(unwantedTentacle.gameObject);
        }

        private Tentacle CreateTentacle(Vector2 origin)
        {
            Tentacle tentacle = Instantiate(_tentaclePrefab, transform);
            TentaclePoint basePoint = new TentaclePoint(gameObject, origin);
            TentaclePoint topPoint = new TentaclePoint(s_tentacleHits[0].collider.gameObject, s_tentacleHits[0].point);

            tentacle.Construct(basePoint, topPoint, _settings.Stiffness, _settings.Damp, _settings.ForceVerticalOffset);
            return tentacle;
        }

        private void RemoveUnwantedTentacle()
        {
            float minDesirability = GetMinTentacleDesirability(out Tentacle unwantedTentacle);

            if (unwantedTentacle == null || minDesirability > _settings.RemoveDesireThreshold)
                return;

            RemoveTentacle(unwantedTentacle);
        }

        private float GetMinTentacleDesirability(out Tentacle unwantedTentacle)
        {
            float minDesirability = float.MaxValue;
            unwantedTentacle = null;

            foreach (Tentacle tentacle in _activeTentacles)
            {
                float desirability = CalculateDesirability(tentacle.TipPoint);

                if (desirability < minDesirability)
                {
                    minDesirability = desirability;
                    unwantedTentacle = tentacle;
                }
            }

            return minDesirability;
        }

        private float CalculateDesirability(Vector2 point)
        {
            Vector2 offset = point - (Vector2) transform.position;
            float distance = -offset.magnitude / _settings.Radius;
            float angle = Vector2.Dot(offset.normalized, _moveDirection);

            return distance + angle;
        }
    }
}