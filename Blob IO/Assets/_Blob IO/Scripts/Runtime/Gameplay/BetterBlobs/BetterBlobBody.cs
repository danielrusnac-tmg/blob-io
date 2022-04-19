using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class BetterBlobBody : MonoBehaviour
    {
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _resolution = 30f;
        [SerializeField] private MeshFilter _meshFilter;
        
        private DeformableBlob _deformableBlob;
        private BlobRenderer _circleRenderer;

        private void Awake()
        {
            int pointCount = GetPointCount();
            
            _deformableBlob = new DeformableBlob(pointCount, _radius);
            _circleRenderer = new BlobRenderer(_meshFilter, pointCount);
        }

        private void Update()
        {
            _deformableBlob.UpdatePoints(Time.deltaTime);
            _circleRenderer.UpdateMesh(_deformableBlob.BlobPoints);
        }

        private int GetPointCount()
        {
            return (int)(_radius * 2 * Mathf.PI / (1 / _resolution));
        }
    }
}