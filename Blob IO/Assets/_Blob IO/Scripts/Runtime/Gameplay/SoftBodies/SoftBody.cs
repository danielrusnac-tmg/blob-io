using System;
using UnityEditor;
using UnityEngine;

namespace BlobIO.Gameplay.SoftBodies
{
    public class SoftBody : MonoBehaviour
    {
        [SerializeField] private LayerMask _solidMask;
        [SerializeField] private float _mass = 10f;
        [SerializeField] private float _resolution = 0.1f;
        [SerializeField] private float _area = 10f;
        [SerializeField] private float _stiffness = 10f;
        [SerializeField] private float _damp = 1f;

        private MassPoint[] _points;
        private Spring[] _springs;

        public void Construct()
        {
            float radius = Mathf.Sqrt(_area / Mathf.PI);
            float perimeter = 2 * Mathf.PI * radius;
            int pointCount = (int) (perimeter / _resolution);
            
            CreatePoints(pointCount, radius);
            CreateSprings(pointCount);
        }

        private void Awake()
        {
            Construct();
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _points.Length; i++)
            {
                _points[i].Force = Physics2D.gravity * _points[i].Mass;
                _points[i].Velocity += _points[i].Force * Time.fixedDeltaTime;
            }

            for (int i = 0; i < _points.Length; i++)
            {
                _points[i].Position += _points[i].Velocity * Time.fixedDeltaTime;
            }
            
            for (int i = 0; i < _points.Length; i++)
            {
                Collider2D overlapCollider = Physics2D.OverlapPoint(_points[i].Position, _solidMask);

                if (overlapCollider != null)
                {
                    _points[i].Position -= _points[i].Velocity * Time.fixedDeltaTime;
                }
            }
        }

        private void CreatePoints(int pointCount, float radius)
        {
            float pointMass = pointCount / _mass;
            float angleStep = 360f / pointCount;

            _points = new MassPoint[pointCount];
            Vector2 center = transform.position;

            for (int i = 0; i < pointCount; i++)
            {
                Vector2 offset = Quaternion.AngleAxis(angleStep * i, Vector3.forward) * Vector2.right * radius;
                _points[i] = new MassPoint(center + offset, pointMass);
            }
        }

        private void CreateSprings(int pointCount)
        {
            _springs = new Spring[pointCount * 2];
            int springIndex = 0;

            for (int i = 0; i < pointCount; i++)
            {
                _springs[springIndex] = new Spring(i, (int) Mathf.Repeat(i - 1, pointCount));
                _springs[springIndex + 1] = new Spring(i, (int) Mathf.Repeat(i + 1, pointCount));

                springIndex += 2;
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (_springs == null || _springs.Length == 0)
                return;
            
            for (var i = 0; i < _springs.Length - 1; i += 2)
            {
                Handles.DrawSolidDisc(_points[_springs[i].PointIndexA].Position, Vector3.forward, 0.02f);
                Handles.DrawLine(_points[_springs[i].PointIndexA].Position, _points[_springs[i].PointIndexB].Position);
            }
#endif
        }
    }
}