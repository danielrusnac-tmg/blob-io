using System.Collections.Generic;
using UnityEngine;

namespace BlobIO.SoftBody
{
    public class SoftBodyPoint : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _pointRigidbody;
        
        private readonly Dictionary<SoftBodyPoint, SoftBodyConnection> _connectionBtPoint =
            new Dictionary<SoftBodyPoint, SoftBodyConnection>();

        public Vector3 Position => _pointRigidbody.position;
        public Vector3 Normal { get; set; }
            
        public void Connect(SoftBodyPoint point, SoftBodySpringSetting setting)
        {
            _connectionBtPoint.Add(point, CreateConnection(point, setting));
        }
        
        public void Disconnect(SoftBodyPoint point)
        {
            _connectionBtPoint[point].Cleanup();
            _connectionBtPoint.Remove(point);
        }

        public void AddForce(Vector3 force)
        {
            _pointRigidbody.AddForce(force * _pointRigidbody.mass);
        }
        
        private SoftBodyConnection CreateConnection(SoftBodyPoint point, SoftBodySpringSetting setting)
        {
            return new SoftBodyConnection(_pointRigidbody, point._pointRigidbody, setting);
        }
    }
}