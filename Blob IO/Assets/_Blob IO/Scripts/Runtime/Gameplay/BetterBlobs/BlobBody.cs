using System;
using UnityEngine;

namespace BlobIO.BetterBlobs
{
    [Serializable]
    public class BlobBodySettings
    {
        [SerializeField] private float _floorY;
        [SerializeField] private float _pressure;
        [SerializeField] private float _stiffness;
        [SerializeField] private float _damp;

        public float FloorY => _floorY;
        public float Pressure => _pressure;
        public float Stiffness => _stiffness;
        public float Damp => _damp;
    }
    
    public class BlobBody
    {
        public readonly BlobPoint[] Points;
        public readonly Spring[] Springs;
        
        private readonly BlobRenderer _blobRenderer;
        private readonly BlobBodySettings _setting;

        private float Stiffness => _setting.Stiffness;
        private float Damp => _setting.Damp;
        private float Pressure => _setting.Pressure;
        private float FloorY => _setting.FloorY;

        public BlobBody(int pointCount, float radius, BlobRenderer blobRenderer, BlobBodySettings setting)
        {
            _blobRenderer = blobRenderer;
            _setting = setting;
            Points = CreatePoints(pointCount, radius, Vector3.zero);;
            Springs = CreateSprings(pointCount);
        }

        public void AddVelocity(Vector3 velocityDelta)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].Velocity = velocityDelta;
            }
        }

        public void UpdatePoints(float deltaTime)
        {
            // gravity force
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].Force = Physics2D.gravity;
            }

            // spring force
            for (var i = 0; i < Springs.Length; i++)
            {
                Vector3 a = Points[Springs[i].A].Position;
                Vector3 b = Points[Springs[i].B].Position;

                Vector3 offset = a - b;
                float distance = offset.magnitude;
                
                if (distance==0)
                    continue;

                Vector3 velocity = Points[Springs[i].A].Velocity - Points[Springs[i].B].Velocity;
                Vector3 springForce = Stiffness * (distance - Springs[i].Length) * offset / distance;
                Vector3 dampForce = velocity * Damp;
                Vector3 force = springForce + dampForce;

                Points[Springs[i].A].Force -= force;
                Points[Springs[i].B].Force += force;
                Springs[i].Normal = Vector3.Cross(Vector3.forward, offset.normalized);
            }
            
            // pressure force
            for (int i = 0; i < Springs.Length; i++)
            {
                Vector2 a = Points[Springs[i].A].Position;
                Vector2 b = Points[Springs[i].B].Position;
                
                Vector2 offset = a - b;
                float distance = offset.magnitude;
                float pressure = distance * Pressure * (1f / GetArea());

                Points[Springs[i].A].Force += Springs[i].Normal * pressure;
                Points[Springs[i].B].Force += Springs[i].Normal * pressure;
            }
            
            // move
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].Velocity += Points[i].Force * deltaTime;
                Vector3 movement = Points[i].Velocity * deltaTime;
                Points[i].Position += movement;
                
                float dry = Points[i].Velocity.y * deltaTime;

                if (Points[i].Position.y + dry < FloorY)
                {
                    dry = FloorY - Points[i].Position.y;
                    Points[i].Velocity.y *= -0.1f;
                }

                Points[i].Position.y += dry;
            }
        }

        private float GetArea()
        {
            float area = 0f;
            
            for (int i = 0; i < _blobRenderer.Triangles.Length; i += 3)
            {
                area += GetTriangleArea(
                    _blobRenderer.Vertices[_blobRenderer.Triangles[i + 0]],
                    _blobRenderer.Vertices[_blobRenderer.Triangles[i + 1]],
                    _blobRenderer.Vertices[_blobRenderer.Triangles[i + 2]]);
            }
            
            return area;
        }

        private float GetTriangleArea(Vector3 a, Vector3 b, Vector3 c)
        {
            return Mathf.Abs(a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)) * 0.5f;
        }

        private BlobPoint[] CreatePoints(int count, float radius, Vector3 center)
        {
            BlobPoint[] result = new BlobPoint[count];
            float angleStep = 360f / count;

            for (int i = 0; i < count; i++)
            {
                Vector3 position = center + Quaternion.AngleAxis(angleStep * i, Vector3.forward) * Vector3.right * radius;
                Vector3 normal = (position - center).normalized;

                result[i].Position = position;
                result[i].Normal = normal;
            }

            return result;
        }

        private Spring[] CreateSprings(int count)
        {
            Spring[] result = new Spring[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = CreateSpring(i, (int) Mathf.Repeat(i + 1, count));
            }
            
            return result;
        }

        private Spring CreateSpring(int a, int b)
        {
            return new Spring
            {
                A = a,
                B = b,
                Length = Vector2.Distance(Points[a].Position, Points[b].Position)
            };
        }
    }
}