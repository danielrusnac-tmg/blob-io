using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class BetterBlobVision : MonoBehaviour
    {
        private static readonly RaycastHit2D[] s_tentacleHits;
        
        [SerializeField] private LayerMask _grabMask;
        [SerializeField] private int _tryCount = 20;
        [SerializeField] private float _rayRadius = 15f;
        [SerializeField] private float _angleRange = 360f;
        [SerializeField] private AnimationCurve _angleBias;

        private Vector2[] _hitPoints;

        static BetterBlobVision()
        {
            s_tentacleHits = new RaycastHit2D[1];
        }

        private void Awake()
        {
            _hitPoints = new Vector2[_tryCount];
        }

        public int TryGetValidPoints(Vector2 direction, out Vector2[] points)
        {
            int hitCount = 0;
            Vector2 origin = transform.position;
            
            for (int i = 0; i < _tryCount; i++)
            {
                float angle = GetRandomAngleOffset();
                Vector2 rayDirection = Quaternion.AngleAxis(angle, Vector3.forward) * direction;

                if (Physics2D.RaycastNonAlloc(origin, rayDirection, s_tentacleHits,_rayRadius, _grabMask) > 0)
                {
                    _hitPoints[hitCount] = s_tentacleHits[0].point;
                    hitCount++;
                }
            }

            points = _hitPoints;
            return hitCount;
        }
        
        private float GetRandomAngleOffset()
        {
            return _angleBias.Evaluate(Random.value) * _angleRange * 0.5f;
        }
    }
}