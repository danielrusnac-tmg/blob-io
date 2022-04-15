using UnityEngine;

namespace BlobIO.Gameplay.SoftBodies
{
    public struct MassPoint
    {
        public float Mass;
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Force;

        public MassPoint(Vector2 position, float mass)
        {
            Mass = mass;
            Position = position;
            Velocity = Vector2.zero;
            Force = Vector2.zero;
        }
    }
}