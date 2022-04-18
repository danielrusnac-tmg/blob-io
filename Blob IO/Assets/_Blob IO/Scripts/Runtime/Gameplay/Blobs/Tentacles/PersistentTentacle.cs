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

        private float _weight = 1f;
        private bool _isGrabbing;
        private TentaclePoint _basePoint;
        private TentaclePoint _grabPoint;
        private Tentacle _tentacle;
        
        public float Radius => _setting.Radius * _weight;
        public float StepDistance => _setting.StepDistance * _weight;
        public float ReleaseDistance => _setting.ReleaseDistance * _weight;

        private Vector2 GetTentacleOrigin => transform.position;
        private Vector2 GetIdealTentacleTip => transform.position + transform.up * Radius;

        public Vector2 GetTentacleDirection => transform.up;
        public bool IsGrabbing => _isGrabbing;
        public float Stretchiness => Vector2.Distance(GetTentacleOrigin, _grabPoint.Position) / Radius;
        public Vector2 TipPosition => _grabPoint.Position;

        static PersistentTentacle()
        {
            s_tentacleHits = new RaycastHit2D[1];
        }

        public void Construct(TentaclePoint basePoint)
        {
            Construct(basePoint, _setting);
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

            if (IsFacingWall(out Vector2 point, out GameObject grabbedObject) && ShouldGrab())
            {
                Grab(point, grabbedObject);
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
            _weight = weight;
            
            if (_tentacle == null)
                return;
            
            _tentacle.Weight = weight;
        }

        public void Grab(Vector2 point, GameObject target)
        {
            if (_isGrabbing)
                Release();
            
            _grabPoint = new TentaclePoint(target, point);
            _isGrabbing = true;
            AttachSpring();
        }
        
        public bool ShouldGrab()
        {
            return !_isGrabbing || Vector2.Distance(s_tentacleHits[0].point, _grabPoint.Position) > StepDistance;
        }

        public bool IsFacingWall(out Vector2 point, out GameObject grabbedObject)
        {
            int collisionCount = Physics2D.RaycastNonAlloc(GetTentacleOrigin, GetTentacleDirection, s_tentacleHits, Radius, _setting.GrabMask);

            if (collisionCount == 0)
            {
                point = GetTentacleOrigin;
                grabbedObject = null;
                return false;
            }
            
            point = s_tentacleHits[0].point;
            grabbedObject = s_tentacleHits[0].collider.gameObject;
            
            return true;
        }

        public bool ShouldRelease()
        {
            if (!_isGrabbing)
                return false;
            
            Vector2 offset = _grabPoint.Position - GetTentacleOrigin;
            
            return Vector2.Distance(GetTentacleOrigin, _grabPoint.Position) > ReleaseDistance ||
                   Physics2D.RaycastNonAlloc(GetTentacleOrigin, offset.normalized, s_tentacleHits, offset.magnitude - 0.1f,
                       _setting.SolidMask) > 0;
        }

        public void Release()
        {
            _isGrabbing = false;
            RemoveSpring();
        }

        private void AttachSpring()
        {
            _tentacle = Instantiate(_tentaclePrefab, transform);
            _tentacle.Construct(_basePoint, _grabPoint, _stiffness, _damp, 0.5f);
        }

        private void RemoveSpring()
        {
            _tentacle.Despawn();
        }
    }
}