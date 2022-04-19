using System;
using System.Threading.Tasks;
using UnityEngine;

namespace BlobIO.BetterBlobs
{
    [Serializable]
    public class BlobBody
    {
        [SerializeField] private float _gasConstant = 50f;
        [SerializeField] private float _pressure;

        private readonly BlobRenderer _blobRenderer;
        private BlobPoint[] _blobPoints;

        public BlobPoint[] BlobPoints => _blobPoints;

        public BlobBody(int pointCount, float radius, BlobRenderer blobRenderer)
        {
            _blobRenderer = blobRenderer;
            _blobPoints = CreatePoints(pointCount, radius, Vector3.zero);;
        }

        public void UpdatePoints(float deltaTime)
        {
            CalculateForces();
            MovePoints();
        }

        private void CalculateForces()
        {
            _pressure = GetPressure();
            
            for (int i = 0; i < _blobPoints.Length; i++)
            {
                _blobPoints[i].ResetForce();
                _blobPoints[i].AddForce(_blobPoints[i].Normal * _pressure);
            }
        }

        private void MovePoints()
        {
            for (int i = 0; i < _blobPoints.Length; i++)
            {
                _blobPoints[i].CalculateVelocity(Time.deltaTime);
                _blobPoints[i].UpdatePosition(Time.deltaTime);
            }
        }

        private float GetPressure()
        {
            return Mathf.Pow(Mathf.Pow(GetArea(), 2), -1) * _gasConstant;
        }

        private float GetArea()
        {
            float area = 0f;
            
            for (int i = 0; i < _blobRenderer.Triangles.Length; i += 3)
            {
                area += GetTriangleArea(
                    _blobRenderer.Vertices[_blobRenderer.Triangles[i + 0]],
                    _blobRenderer.Vertices[_blobRenderer.Triangles[i + 1]],
                    _blobRenderer.Vertices[_blobRenderer.Triangles[i + 2]]);
            }
            
            return area;
        }

        private float GetTriangleArea(Vector3 a, Vector3 b, Vector3 c)
        {
            return Mathf.Abs(a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)) * 0.5f;
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