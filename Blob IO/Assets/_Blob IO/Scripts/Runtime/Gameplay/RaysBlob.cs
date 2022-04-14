using UnityEngine;

namespace BlobIO.Gameplay
{
    public class RaysBlob : MonoBehaviour
    {
        [SerializeField] private int _tentacleCount = 10;
        [SerializeField] private float _radius = 5f;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private Spring _spring;

        private Rigidbody2D _rigidbody2D;
        private Tentacle[] _tentacles;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            CreateTentacles();
        }

        private void FixedUpdate()
        {
            Vector2 force = CalculateTentacleForce(Time.fixedDeltaTime);
            _rigidbody2D.AddForce(force * _rigidbody2D.mass);
        }

        private void CreateTentacles()
        {
            _tentacles = new Tentacle[_tentacleCount];
            float angle = 360f / _tentacleCount;

            for (int i = 0; i < _tentacleCount; i++)
                _tentacles[i] = new Tentacle(angle * i, _radius, _wallLayer, new Spring(_spring));
        }

        private Vector2 CalculateTentacleForce(float deltaTime)
        {
            Vector2 center = transform.position;
            Vector2 force = Vector2.zero;

            foreach (Tentacle tentacle in _tentacles)
                force += tentacle.CalculateForce(center, deltaTime);

            return force / _tentacleCount;
        }
    }
}
