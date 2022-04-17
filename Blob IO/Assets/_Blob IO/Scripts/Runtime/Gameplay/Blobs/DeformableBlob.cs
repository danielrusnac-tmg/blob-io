using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.Blobs
{
    public class DeformableBlob : MonoBehaviour, IControllable
    {
        [SerializeField] private int _pointCount = 32;
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private MeshFilter _meshFilter;

        private CircleRenderer _circleRenderer;
        private IControllableInput _input;

        private void Awake()
        {
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
            _circleRenderer.UpdateMesh(GenerateCircleVertices(_pointCount, _radius, Vector3.zero));
        }

        private Vector3[] GenerateCircleVertices(int pointCount, float radius, Vector3 center)
        {
            Vector3[] result = new Vector3[pointCount];

            float angleStep = 360f / pointCount;

            for (int i = 0; i < pointCount; i++)
                result[i] = center + Quaternion.AngleAxis(angleStep * i, Vector3.forward) * Vector3.right * radius;
            
            return result;
        }
    }
}