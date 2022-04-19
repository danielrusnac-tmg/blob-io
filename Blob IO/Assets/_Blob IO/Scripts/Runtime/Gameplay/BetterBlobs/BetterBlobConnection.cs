using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class BetterBlobConnection
    {
        private readonly SpringJoint2D _spring;
        
        public BetterBlobConnection(Rigidbody2D a, Rigidbody2D b, BodySpringSetting setting)
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