using System.Collections.Generic;
using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class BetterBlobPoint : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _pointRigidbody;

        private readonly Dictionary<BetterBlobPoint, BetterBlobConnection> _connectionBtPoint =
            new Dictionary<BetterBlobPoint, BetterBlobConnection>();

        public void Connect(BetterBlobPoint point, BodySpringSetting setting)
        {
            _connectionBtPoint.Add(point, CreateConnection(point, setting));
        }

        public void Disconnect(BetterBlobPoint point)
        {
            _connectionBtPoint[point].Cleanup();
            _connectionBtPoint.Remove(point);
        }
        
        private BetterBlobConnection CreateConnection(BetterBlobPoint point, BodySpringSetting setting)
        {
            return new BetterBlobConnection(_pointRigidbody, point._pointRigidbody, setting);
        }
    }
}