using System.Linq;
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

        [SerializeField] private SmartTentacleSetting _setting;
        [SerializeField] private Seeker _seeker;

        private Vector2 _idealGrabPosition;
        private GrabPoint _currentGrabPoint;
        private Transform _tentacleBase;

        public float GrabAngle { get; set; }
        private Vector2 GrabDirection => Quaternion.AngleAxis(GrabAngle, Vector3.forward) * _tentacleBase.up;

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
            // Handles.DrawSolidDisc(transform.position, Vector3.forward, 0.1f);
            //
            // if (_setting == null)
            //     return;
            //
            // Handles.DrawLine(transform.position, transform.position + transform.up * _setting.Radius);
            
            if (!Application.isPlaying)
                return;

            Handles.color = Color.yellow;
            Handles.DrawSolidDisc(_idealGrabPosition, Vector3.forward, 0.1f);

            Handles.color = Color.blue;
            Handles.DrawSolidDisc(_currentGrabPoint.Position, Vector3.forward, 0.1f);
            // Handles.DrawLine(_tip.position, _currentGrabPoint.Position);
#endif
        }

        private void UpdateGrabPosition()
        {
            _idealGrabPosition = GetIdealGrabPosition();
            _currentGrabPoint = CreateGrabPoint(_currentGrabPoint.Position);
            _seeker.StartPath(_tentacleBase.position, _idealGrabPosition, OnPathCalculated);
        }

        private void OnPathCalculated(Path path)
        {
            GrabPoint randomGrabPoint = GetRandomGrabPoint(path);

            if (randomGrabPoint.Score > _currentGrabPoint.Score && Vector2.Distance(randomGrabPoint.Position, _currentGrabPoint.Position) > _setting.StepDistance)
            {
                _currentGrabPoint = randomGrabPoint;
            }
        }

        private Vector2 GetIdealGrabPosition()
        {
            return (Vector2)_tentacleBase.position + GrabDirection * _setting.Radius;
        }

        private GrabPoint GetRandomGrabPoint(Path path)
        {
            int outerMostPointInRadius = GetOuterMostPointInRadius(path);

            Vector2 randomPosition = path.vectorPath[outerMostPointInRadius];
            
            if (Physics2D.OverlapCircleNonAlloc(randomPosition, _setting.GrabRayRadius, s_grabColliders, _setting.GrabMask) == 0)
                return CreateInvalidGrabPoint(randomPosition);
            
            // if (Physics2D.OverlapPointNonAlloc(randomPosition, s_grabColliders, _setting.GrabMask) > 0)
            //     return CreateInvalidGrabPoint(randomPosition);
            //
            randomPosition = s_grabColliders[0].ClosestPoint(randomPosition);
            
            return CreateGrabPoint(randomPosition);
        }

        private int GetOuterMostPointInRadius(Path path)
        {
            float distance = 0f;
            int outerMostPointInRadius = 0;

            for (var i = 1; i < path.vectorPath.Count; i++)
            {
                float segmentDistance = Vector2.Distance(path.vectorPath[i - 1], path.vectorPath[i]);

                if (distance + segmentDistance > _setting.Radius)
                {
                    outerMostPointInRadius = i - 1;
                    break;
                }

                distance += segmentDistance;
            }

            return outerMostPointInRadius;
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
    }
}