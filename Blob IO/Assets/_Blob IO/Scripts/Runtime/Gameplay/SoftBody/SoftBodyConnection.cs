using UnityEngine;

namespace BlobIO.SoftBody
{
    public class SoftBodyConnection
    {
        private readonly SpringJoint2D _spring;
        
        public SoftBodyConnection(Rigidbody2D a, Rigidbody2D b, SoftBodySpringSetting setting)
        {
            _spring = a.gameObject.AddComponent<SpringJoint2D>();
            _spring.connectedBody = b;
            _spring.frequency = setting.Frequency;
            _spring.dampingRatio = setting.Damping;
            _spring.autoConfigureDistance = false;
            _spring.autoConfigureConnectedAnchor = false;
        }
        public void Cleanup()
        {
            Object.Destroy(_spring);
        }
    }
}