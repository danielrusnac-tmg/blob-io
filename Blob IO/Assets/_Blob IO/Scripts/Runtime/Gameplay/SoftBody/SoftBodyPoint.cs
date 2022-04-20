using UnityEngine;

namespace BlobIO.SoftBody
{
    public class SoftBodyPoint : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _pointRigidbody;
        
        public Vector3 Position => _pointRigidbody.position;
        public Vector3 Velocity => _pointRigidbody.velocity;

        public void AddForce(Vector3 force)
        {
            _pointRigidbody.AddForce(force * _pointRigidbody.mass);
        }
    }
}