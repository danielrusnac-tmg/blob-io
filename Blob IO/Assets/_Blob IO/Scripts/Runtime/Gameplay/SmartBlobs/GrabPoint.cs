using System;
using UnityEngine;

namespace BlobIO.SmartBlobs
{
    [Serializable]
    public struct GrabPoint
    {
        public Vector2 Position;
        public float Score;

        public GrabPoint(Vector2 position, float score)
        {
            Position = position;
            Score = score;
        }
    }
}