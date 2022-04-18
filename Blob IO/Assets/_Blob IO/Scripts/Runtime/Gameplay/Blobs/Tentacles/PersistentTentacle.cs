using System;
using UnityEditor;
using UnityEngine;

namespace BlobIO.Blobs.Tentacles
{
    public class PersistentTentacle : MonoBehaviour
    {
        private static readonly RaycastHit2D[] s_tentacleHits;
        
        [SerializeField] private PersistentTentacleSetting _setting;

        private bool _isGrabbing;
        private TentaclePoint _basePoint;
        private TentaclePoint _grabPoint;
        
        private Vector2 GetTentacleDirection => transform.up;
        private Vector2 GetTentacleOrigin => transform.position;
        private Vector2 GetTentacleTip => transform.position + transform.up * _setting.Radius;

        static PersistentTentacle()
        {
            s_tentacleHits = new RaycastHit2D[1];
        }

        private void Awake()
        {
            _basePoint = new TentaclePoint(gameObject, GetTentacleOrigin);
        }

        private void Update()
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

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (_setting == null)
                return;

            if (!_isGrabbing)
            {
                Handles.DrawLine(GetTentacleOrigin, GetTentacleTip);
            }
            else 
            {
                if (_grabPoint != null)
                {
                    float compression = Vector2.Distance(GetTentacleOrigin, _grabPoint.Position) / _setting.Radius;
                    Handles.color = Color.Lerp(Color.green, Color.red, compression);
                    Handles.DrawLine(GetTentacleOrigin, _grabPoint.Position);
                    Handles.DrawSolidDisc(_grabPoint.Position, Vector3.forward, 0.1f);
                }
            }
#endif
        }

        private void Grab(Vector2 point, GameObject target)
        {
            _grabPoint = new TentaclePoint(target, point);
            _isGrabbing = true;
        }

        private void Release()
        {
            _isGrabbing = false;
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