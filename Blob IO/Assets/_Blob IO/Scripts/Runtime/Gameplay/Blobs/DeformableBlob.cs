using System;
using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.Blobs
{
    public class BlobPoint
    {
        public Vector3 Position;
        public Vector3 Normal;
        
        private Vector3 _velocity;
        private Vector3 _force;

        public BlobPoint(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }

        public void CalculateVelocity(float deltaTime)
        {
            _velocity += _force * deltaTime;
        }

        public void UpdatePosition(float deltaTime)
        {
            Position += _velocity * deltaTime;
        }

        public void ResetForce()
        {
            _force = Vector3.zero;
        }

        public void AddForce(Vector3 force)
        {
            _force += force;
        }
    }

    public class DeformableBlob : MonoBehaviour
    {
        [SerializeField] private int _pointCount = 32;
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private MeshFilter _meshFilter;

        private CircleRenderer _circleRenderer;
        private BlobPoint[] _blobPoints;
        private IControllableInput _input;

        private void Awake()
        {
            _blobPoints = CreatePoints(_pointCount, _radius, Vector3.zero);;
            _circleRenderer = new CircleRenderer(_meshFilter, _pointCount);
        }

        private void Update()
        {
            UpdateCircleMesh();
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }

        private void UpdateCircleMesh()
        {
            // for (int i = 1; i < _blobPoints.Length; i++)
            // {
            //     _blobPoints[i].ResetForce();
            //     _blobPoints[i].AddForce(Physics2D.gravity);
            // }
            //
            // for (int i = 0; i < _blobPoints.Length; i++)
            // {
            //     _blobPoints[i].CalculateVelocity(Time.deltaTime);
            //     _blobPoints[i].UpdatePosition(Time.deltaTime);
            // }

            _circleRenderer.UpdateMesh(_blobPoints);
        }

        private BlobPoint[] CreatePoints(int pointCount, float radius, Vector3 center)
        {
            BlobPoint[] result = new BlobPoint[pointCount];
            float angleStep = 360f / pointCount;

            for (int i = 0; i < pointCount; i++)
            {
                Vector3 position = center + Quaternion.AngleAxis(angleStep * i, Vector3.forward) * Vector3.right * radius;
                Vector3 normal = (position - center).normalized;
                
                result[i] = new BlobPoint(position, normal);
            }

            return result;
        }
    }
}