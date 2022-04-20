using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlobIO.BetterBlobs
{
    public class BetterBlobBody : MonoBehaviour
    {
        [SerializeField] private int _steps = 1;
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _resolution = 30f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _jumpAngle = 30f;
        [SerializeField] private BlobBodySettings _setting;
        [SerializeField] private MeshFilter _meshFilter;
        
        private BlobBody _blobBody;
        private BlobRenderer _circleRenderer;
        
        private void Awake()
        {
            int pointCount = GetPointCount();
            _circleRenderer = new BlobRenderer(_meshFilter, pointCount);
            _blobBody = new BlobBody(pointCount, _radius, _circleRenderer, _setting);
            _circleRenderer.UpdateMesh(_blobBody.Points, _blobBody.Springs);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _blobBody.AddVelocity(_jumpForce *
                                      (Quaternion.AngleAxis(Random.Range(-_jumpAngle, _jumpAngle), Vector3.forward) *
                                       Vector3.up));
            }
        }

        private void FixedUpdate()
        {
            UpdateBody();
        }

        [ContextMenu(nameof(UpdateBody))]
        private void UpdateBody()
        {
            for (var i = 0; i < _steps; i++)
            {
                _blobBody.UpdatePoints(Time.fixedDeltaTime / _steps, transform);    
                _circleRenderer.UpdateMesh(_blobBody.Points, _blobBody.Springs);
            }
        }

        private int GetPointCount()
        {
            return (int)(2 * Mathf.PI * _radius / (1 / _resolution));
        }
    }
}