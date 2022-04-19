using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class DeformableBlob
    {
        private BlobPoint[] _blobPoints;

        public BlobPoint[] BlobPoints => _blobPoints;

        public DeformableBlob(int pointCount, float radius)
        {
            _blobPoints = CreatePoints(pointCount, radius, Vector3.zero);;
        }

        public void UpdatePoints(float deltaTime)
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