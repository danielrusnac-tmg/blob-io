using UnityEditor;
using UnityEngine;

namespace BlobIO.Blobs.Tentacles
{
    public class PersistentTentacle : MonoBehaviour
    {
        private static readonly RaycastHit2D[] s_tentacleHits;

        [SerializeField] private PersistentTentacleSetting _setting;
        [SerializeField] private float _stiffness = 30f;
        [SerializeField] private float _damp = 1f;
        [SerializeField] private Tentacle _tentaclePrefab;

        private bool _isGrabbing;
        private TentaclePoint _basePoint;
        private TentaclePoint _grabPoint;
        private Tentacle _tentacle;

        private Vector2 GetTentacleOrigin => transform.position;
        private Vector2 GetIdealTentacleTip => transform.position + transform.up * _setting.Radius;

        public Vector2 GetTentacleDirection => transform.up;
        public bool IsGrabbing => _isGrabbing;
        public float Stretchiness => Vector2.Distance(GetTentacleOrigin, _grabPoint.Position) / _setting.Radius;
        public Vector2 TipPosition => _grabPoint.Position;

        static PersistentTentacle()
        {
            s_tentacleHits = new RaycastHit2D[1];
        }

        public void Construct(TentaclePoint basePoint, PersistentTentacleSetting setting)
        {
            _basePoint = basePoint;
            _setting = setting;
        }

        public void UpdateTentacle()
        {
            if (ShouldRelease())
            {
                Release();
            }

            if (IsFacingWall() && ShouldGrab())
            {
                Grab(s_tentacleHits[0].point, s_tentacleHits[0].collider.gameObject);
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (_setting == null)
                return;

            if (!_isGrabbing)
            {
                Handles.DrawLine(GetTentacleOrigin, GetIdealTentacleTip);
            }
            else 
            {
                if (_grabPoint != null)
                {
                    Handles.color = Color.Lerp(Color.green, Color.red, Stretchiness);
                    Handles.DrawLine(GetTentacleOrigin, _grabPoint.Position);
                    Handles.DrawSolidDisc(_grabPoint.Position, Vector3.forward, 0.1f);
                }
            }
#endif
        }

        public void SetWeight(float weight)
        {
            if (_isGrabbing)
            {
                _tentacle.Weight = weight;
            }
        }

        private void Grab(Vector2 point, GameObject target)
        {
            if (_isGrabbing)
                Release();
            
            _grabPoint = new TentaclePoint(target, point);
            _isGrabbing = true;
            AttachSpring();
        }

        private void AttachSpring()
        {
            _tentacle = Instantiate(_tentaclePrefab, transform);
            _tentacle.Construct(_basePoint, _grabPoint, _stiffness, _damp, 0f, _setting.Radius);
        }

        private void Release()
        {
            _isGrabbing = false;
            RemoveSpring();
        }

        private void RemoveSpring()
        {
            Destroy(_tentacle.gameObject);
        }

        private bool ShouldGrab()
        {
            return !_isGrabbing || Vector2.Distance(s_tentacleHits[0].point, _grabPoint.Position) > _setting.StepDistance;
        }

        private bool IsFacingWall()
        {
            return Physics2D.RaycastNonAlloc(GetTentacleOrigin, GetTentacleDirection, s_tentacleHits, _setting.Radius, _setting.WallMask) > 0;
        }

        private bool ShouldRelease()
        {
            if (!_isGrabbing)
                return false;
            
            Vector2 offset = _grabPoint.Position - GetTentacleOrigin;
            
            return Vector2.Distance(GetTentacleOrigin, _grabPoint.Position) > _setting.ReleaseDistance ||
                   Physics2D.RaycastNonAlloc(GetTentacleOrigin, offset.normalized, s_tentacleHits, offset.magnitude - 0.1f,
                       _setting.WallMask) > 0;
        }
    }
}