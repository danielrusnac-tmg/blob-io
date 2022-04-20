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
}