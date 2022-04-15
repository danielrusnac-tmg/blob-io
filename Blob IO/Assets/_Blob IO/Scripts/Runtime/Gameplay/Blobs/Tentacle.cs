using UnityEditor;
using UnityEngine;

namespace BlobIO.Gameplay.Blobs
{
    public class Tentacle
    {
        public readonly Vector2 Point;
        
        private readonly Spring _spring;
        private readonly Transform _body;

        public Tentacle(Spring spring, Transform body, Vector2 point)
        {
            _spring = spring;
            _body = body;
            Point = point;
        }

        public Vector2 CalculateForce(float deltaTime)
        {
            return _spring.CalculateForce(Point, _body.position, deltaTime);
        }

        public void DrawGizmos()
        {
#if UNITY_EDITOR
            Handles.color = Color.red;
            Handles.DrawSolidDisc(Point, Vector3.forward, 0.1f);

            Handles.color = Color.Lerp(Color.green, Color.red, _spring.Compression);
            Handles.DrawLine(Point, _body.position);
#endif
        }
    }
}