using UnityEngine;

namespace BlobIO.Gameplay
{
    public class Blob : MonoBehaviour
    {
        [SerializeField] private float _distance = 2f;
        [SerializeField] private Spring _spring;
        [SerializeField] private Rigidbody2D _target;

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Vector2 offset = (_rigidbody2D.position - _target.position).normalized * _distance;
            Vector3 force = _spring.CalculateForce(_target.position + offset, _rigidbody2D.position);
            _rigidbody2D.AddForce(force * _rigidbody2D.mass);
        }
    }
}
