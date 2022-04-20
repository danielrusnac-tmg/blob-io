using UnityEngine;

namespace BlobIO.SoftBody
{
    public class SoftBody : MonoBehaviour
    {
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _resolution = 10f;
        [SerializeField] private float _pressure = 10f;
        [SerializeField] private SoftBodySpringSetting _surfaceSpring;
        [SerializeField] private SoftBodyPoint _pointPrefab;
        [SerializeField] private SoftBodyRenderer _softBodyRenderer;

        private SoftBodyPoint[] _points;
        
        private void Awake()
        {
            int count = GetPointCount();
            
            CreatePoints(count);
            CreateConnections(count);

            _softBodyRenderer.CreateMesh(_points);
        }

        private void FixedUpdate()
        {
            float volume = GetVolume();

            for (int i = 0; i < _points.Length; i++)
            {
                Vector3 toPrevious = _points[GetPointIndex(i - 1, _points.Length)].Position - _points[i].Position;
                Vector3 toNext = _points[i].Position - _points[GetPointIndex(i + 1, _points.Length)].Position;
                Vector3 offset = (toPrevious + toNext) / 2;
                
                _points[i].Normal = Vector3.Cross(Vector3.forward, offset.normalized);
                _points[i].AddForce(offset.magnitude * _pressure * (1f / volume) * _points[i].Normal);
            }

            _softBodyRenderer.UpdateMesh(_points);
        }

        private float GetVolume()
        {
            return Mathf.Pow(GetCircumference(), 3) / (6 * Mathf.Pow(Mathf.PI, 2));
        }

        private float GetCircumference()
        {
            float circumference = 0f;

            for (int i = 1; i < _points.Length; i++)
            {
                circumference += Vector2.Distance(_points[i - 1].Position, _points[i].Position);
            }

            return circumference;
        }

        private int GetPointCount()
        {
            return (int)(2 * Mathf.PI * _radius / (1 / _resolution));
        }
        
        private void CreatePoints(int count)
        {
            _points = new SoftBodyPoint[count];
            float angleStep = 360f / count;
            
            for (int i = 0; i < count; i++)
            {
                Vector3 position = transform.position + Quaternion.AngleAxis(angleStep * i, Vector3.forward) * Vector3.right * _radius;
                _points[i] = Instantiate(_pointPrefab, position, Quaternion.identity, transform);
            }
        }

        private void CreateConnections(int count)
        {
            for (int i = 0; i < count; i++)
                _points[i].Connect(_points[GetPointIndex(i + 1, count)], _surfaceSpring);
        }

        private int GetPointIndex(int index, int count)
        {
            return (int) Mathf.Repeat(index, count);
        } 
    }
}