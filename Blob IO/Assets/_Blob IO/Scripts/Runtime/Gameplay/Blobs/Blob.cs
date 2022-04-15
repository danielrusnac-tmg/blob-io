using System.Collections;
using System.Collections.Generic;
using BlobIO.Gameplay.Blobs.Tentacles;
using BlobIO.Gameplay.Controllers;
using UnityEditor;
using UnityEngine;

namespace BlobIO.Gameplay.Blobs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Blob : MonoBehaviour, IControllable
    {
        private const int MAX_TRY_POINT = 3;

        [SerializeField] private float _blobRadius = 0.5f;
        [SerializeField] private Tentacle _tentaclePrefab;

        private bool _hasAController;
        private bool _isMoving;
        private Vector2 _moveDirection = Vector2.right;
        private static RaycastHit2D[] _tentacleHits;
        private GlobalSettings _globalSettings;
        private BlobSettings _blobSettings;
        private IControllableInput _input;
        private List<Tentacle> _activeTentacles = new List<Tentacle>();

        static Blob()
        {
            _tentacleHits = new RaycastHit2D[1];
        }

        public void Construct(GlobalSettings globalSettings, BlobSettings blobSettings)
        {
            _globalSettings = globalSettings;
            _blobSettings = blobSettings;
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
                TryAnnNewTentacle();
        }

        private void TryAnnNewTentacle()
        {
            for (int i = 0; i < MAX_TRY_POINT; i++)
            {
                float angle = _blobSettings.GetRandomAngleOffset();
                Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * _moveDirection;
                Vector2 origin = (Vector2)transform.position + direction * _blobRadius;

                if (Physics2D.RaycastNonAlloc(origin, direction, _tentacleHits, _blobSettings.Radius, _globalSettings.WallMask) > 0)
                    _activeTentacles.Add(CreateTentacle(origin));
            }
        }

        private Tentacle CreateTentacle(Vector2 origin)
        {
            Tentacle tentacle = Instantiate(_tentaclePrefab, transform);
            TentaclePoint basePoint = new TentaclePoint(gameObject, origin);
            TentaclePoint topPoint = new TentaclePoint(_tentacleHits[0].collider.gameObject, _tentacleHits[0].point);

            tentacle.Construct(basePoint, topPoint, _blobSettings.Stiffness, _blobSettings.Damp);
            return tentacle;
        }

        private void RemoveUnwantedTentacle()
        {
            float minDesirability = float.MaxValue;
            Tentacle unwantedTentacle = null;
            
            foreach (Tentacle tentacle in _activeTentacles)
            {
                float desirability = CalculateDesirability(tentacle.TipPoint);

                if (desirability < minDesirability)
                {
                    minDesirability = desirability;
                    unwantedTentacle = tentacle;
                }
            }
            
            if (unwantedTentacle == null || minDesirability > _blobSettings.RemoveDesireThreshold)
                return;

            _activeTentacles.Remove(unwantedTentacle);
            Destroy(unwantedTentacle.gameObject);
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