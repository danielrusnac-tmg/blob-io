using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class BetterBlobBody : MonoBehaviour
    {
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _resolution = 30f;
        [SerializeField] private float _pressure = 1f;
        [SerializeField] private float _stiffness = 100f;
        [SerializeField] private float _damp = 10f;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private BlobBody _blobBody;
        
        private BlobRenderer _circleRenderer;
        
        private void Awake()
        {
            int pointCount = GetPointCount();
            _circleRenderer = new BlobRenderer(_meshFilter, pointCount);
            _blobBody = new BlobBody(pointCount, _radius, _circleRenderer, _pressure, _stiffness, _damp);
            _circleRenderer.UpdateMesh(_blobBody.Points);
        }

        private void FixedUpdate()
        {
            UpdateBody();
        }

        [ContextMenu(nameof(UpdateBody))]
        private void UpdateBody()
        {
            _blobBody.UpdatePoints(Time.deltaTime);
            _circleRenderer.UpdateMesh(_blobBody.Points);
        }

        private int GetPointCount()
        {
            return (int)(2 * Mathf.PI * _radius / (1 / _resolution));
        }
    }
}