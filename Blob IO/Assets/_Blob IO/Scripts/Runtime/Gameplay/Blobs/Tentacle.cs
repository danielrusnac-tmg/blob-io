using UnityEngine;

namespace BlobIO.Gameplay.Blobs
{
    public class Tentacle
    {
        private readonly Transform _body;
        private readonly Vector2 _point;

        public float Desirability { get; set; }

        public Tentacle(Transform body, Vector2 point)
        {
            _body = body;
            _point = point;
        }
    }
}