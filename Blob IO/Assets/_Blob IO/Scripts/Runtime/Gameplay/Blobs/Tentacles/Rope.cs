using System;
using UnityEngine;

namespace BlobIO.Blobs.Tentacles
{
    public class Rope : MonoBehaviour
    {
        [SerializeField] private float _length = 1f;
        [SerializeField] private float _resolution = 30f;
        [SerializeField] private int _constrainSteps = 50;
        [SerializeField] private Vector3 _gravity;
        [SerializeField] private LineRenderer _lineRenderer;

        private int _pointCount;
        private float _pointDistance;
        private Vector3[] _points;

        private void Awake()
        {
            CreatePoints();
        }

        private void Update()
        {
            ApplyGravity();

            for (var i = 0; i < _constrainSteps; i++)
                ApplyConstrains();

            _lineRenderer.SetPositions(_points);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawRay(transform.position, -transform.up * _length);
        }

        private void CreatePoints()
        {
            _pointCount = (int) (_length / (1f / _resolution));
            _pointDistance = _length / _pointCount;
            _points = new Vector3[_pointCount];
            _lineRenderer.positionCount = _pointCount;
        }

        private void ApplyConstrains()
        {
            for (int i = 0; i < _pointCount - 1; i++)
            {
                float distance = (_points[i] - _points[i + 1]).magnitude;
                float error = distance - _pointDistance;
                Vector3 changeDir = (_points[i] - _points[i + 1]).normalized;
                Vector3 changeAmount = changeDir * error;

                if (i != 0)
                {
                    _points[i] -= changeAmount * 0.5f;
                    _points[i + 1] += changeAmount * 0.5f;
                }
                else
                {
                    _points[i + 1] += changeAmount;
                }
            }
        }

        private void ApplyGravity()
        {
            for (int i = 0; i < _pointCount; i++)
            {
                _points[i] += _gravity * Time.deltaTime;
            }

            _points[0] = transform.position;
        }
    }
}