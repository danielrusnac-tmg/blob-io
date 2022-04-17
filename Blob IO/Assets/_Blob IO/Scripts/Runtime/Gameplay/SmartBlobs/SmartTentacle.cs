using BlobIO.Blobs.Tentacles;
using Pathfinding;
using UnityEditor;
using UnityEngine;

namespace BlobIO.SmartBlobs
{
    public class SmartTentacle : MonoBehaviour
    {
        private static readonly Collider2D[] s_grabColliders;
        private static readonly RaycastHit2D[] s_rayHits;

        [SerializeField] private float _stiffness = 50f;
        [SerializeField] private float _damp = 0.6f;
        [SerializeField] private GameObject _blob;
        [SerializeField] private Tentacle _tentaclePrefab;
        [SerializeField] private SmartTentacleSetting _setting;
        [SerializeField] private Seeker _seeker;

        private Vector2 _idealGrabPosition;
        private GrabPoint _currentGrabPoint;
        private Transform _tentacleBase;
        private Tentacle _tentacle;

        static SmartTentacle()
        {
            s_grabColliders = new Collider2D[1];
            s_rayHits = new RaycastHit2D[1];
        }

        private void Awake()
        {
            _tentacleBase = transform;
            _currentGrabPoint = CreateInvalidGrabPoint();
        }

        private void Update()
        {
            UpdateGrabPosition();
        }

        private void OnDestroy()
        {
            _seeker.OnDestroy();
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;

            Handles.color = Color.yellow;
            Handles.DrawSolidDisc(_idealGrabPosition, Vector3.forward, 0.1f);

            Handles.color = Color.blue;
            Handles.DrawSolidDisc(_currentGrabPoint.Position, Vector3.forward, 0.1f);
            // Handles.DrawLine(_tip.position, _currentGrabPoint.Position);
#endif
        }

        [ContextMenu(nameof(UpdateGrabPosition))]
        private void UpdateGrabPosition()
        {
            _idealGrabPosition = GetIdealGrabPosition();
            _currentGrabPoint = CreateGrabPoint(_currentGrabPoint.Position);
            GrabPoint randomGrabPoint = GetRandomGrabPoint();

            if (randomGrabPoint.Score > _currentGrabPoint.Score && Vector2.Distance(randomGrabPoint.Position, _currentGrabPoint.Position) > _setting.StepDistance)
            {
                _currentGrabPoint = randomGrabPoint;

                if (_tentacle != null)
                    Destroy(_tentacle.gameObject);

                _tentacle = CreateTentacle(_tentacleBase.position, s_grabColliders[0].gameObject, randomGrabPoint.Position);
                // _seeker.StartPath(_tentacleBase.position, _currentGrabPoint.Position, OnPathCalculated);
            }
        }

        private void OnPathCalculated(Path path)
        {
            
        }

        private Vector2 GetIdealGrabPosition()
        {
            return _tentacleBase.position + _tentacleBase.up * _setting.Radius;
        }

        private GrabPoint GetRandomGrabPoint()
        {
            Vector2 randomPosition = (Vector2) _tentacleBase.position + Random.insideUnitCircle * _setting.Radius;

            if (Physics2D.OverlapCircleNonAlloc(randomPosition, _setting.GrabRayRadius, s_grabColliders, _setting.GrabMask) == 0)
                return CreateInvalidGrabPoint(randomPosition);
            
            if (Physics2D.OverlapPointNonAlloc(randomPosition, s_grabColliders, _setting.GrabMask) > 0)
                return CreateInvalidGrabPoint(randomPosition);

            randomPosition = s_grabColliders[0].ClosestPoint(randomPosition);
            
            return CreateGrabPoint(randomPosition);
        }

        private GrabPoint CreateInvalidGrabPoint(Vector2 position = default)
        {
            return new GrabPoint(position, float.NegativeInfinity);
        }

        private GrabPoint CreateGrabPoint(Vector2 position)
        {
            return new GrabPoint
            {
                Position = position,
                Score = -(_idealGrabPosition - position).sqrMagnitude
            };
        }
        
        private Tentacle CreateTentacle(Vector2 origin, GameObject target, Vector2 targetPoint)
        {
            Tentacle tentacle = Instantiate(_tentaclePrefab, transform);
            TentaclePoint basePoint = new TentaclePoint(_blob, origin);
            TentaclePoint topPoint = new TentaclePoint(target, targetPoint);

            tentacle.Construct(basePoint, topPoint, _stiffness, _damp, 0f);
            return tentacle;
        }
    }
}