using UnityEngine;

namespace BlobIO.SoftBody
{
    public class SoftBodySpring
    {
        private readonly float _length;
        private readonly SoftBodyPoint _a;
        private readonly SoftBodyPoint _b;
        private readonly SoftBodySpringSetting _setting;
        
        private float Damp => _setting.Damping;
        private float Stiffness => _setting.Stiffness;
        public Vector3 Normal { get; private set; }

        public SoftBodySpring(SoftBodyPoint a, SoftBodyPoint b, SoftBodySpringSetting setting)
        {
            _a = a;
            _b = b;
            _setting = setting;

            _length = Vector3.Distance(a.Position, b.Position);
        }

        public void ApplyPressure(float pressure, float volume)
        {
            Vector3 pressureForce = (_a.Position - _b.Position).magnitude * pressure * (1f / volume) * Normal;
            
            _a.AddForce(pressureForce);
            _b.AddForce(pressureForce);
        }
        
        public void ApplyForceToPoints()
        {
            Vector3 a = _a.Position;
            Vector3 b = _b.Position;

            Vector3 offset = a - b;
            float distance = offset.magnitude;

            if (distance == 0)
                return;

            Vector3 velocity = _a.Velocity - _b.Velocity;
            Vector3 springForce = Stiffness * (distance - _length) * offset / distance;
            Vector3 dampForce = velocity * Damp;
            Vector3 force = springForce + dampForce;

            _a.AddForce(-force);
            _b.AddForce(force);
                
            Normal = Vector3.Cross(Vector3.forward, offset.normalized);
        }
    }

    public class SoftBody : MonoBehaviour
    {
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _resolution = 10f;
        [SerializeField] private float _pressure = 10f;
        [SerializeField] private SoftBodySpringSetting _surfaceSpring;
        [SerializeField] private SoftBodyPoint _pointPrefab;
        [SerializeField] private SoftBodyRenderer _softBodyRenderer;

        private SoftBodyPoint[] _points;
        private SoftBodySpring[] _springs;
        
        private void Awake()
        {
            int count = GetPointCount();
            
            CreatePoints(count);

            _springs = new SoftBodySpring[count];
            
            for (int i = 0; i < count; i++)
            {
                _springs[i] = new SoftBodySpring(_points[i], _points[GetPointIndex(i + 1, count)], _surfaceSpring);
            }
            
            _softBodyRenderer.CreateMesh(_points);
        }

        private void FixedUpdate()
        {
            float volume = GetVolume();

            for (int i = 0; i < _springs.Length; i++)
            {
                _springs[i].ApplyForceToPoints();
                _springs[i].ApplyPressure(_pressure, volume);
            }

            _softBodyRenderer.UpdateMesh(_points, _springs);
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

        private int GetPointIndex(int index, int count)
        {
            return (int) Mathf.Repeat(index, count);
        } 
    }
}