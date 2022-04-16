using System;
using System.Collections;
using System.Linq;
using Pathfinding;
using UnityEditor;
using UnityEngine;

namespace BlobIO.SmartBlobs
{
    public class SmartTentacle : MonoBehaviour
    {
        private const float UPDATE_GRAB_POSITION_STEP = 0.1f;

        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _acceleration = 100f;
        [SerializeField] private float _radius = 8f;
        [SerializeField] private Seeker _seeker;
        [SerializeField] private Rigidbody2D _tip;

        private Vector2 _currentGrabPosition;
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Start()
        {
            StartCoroutine(UpdateGrabPositionRoutine());
        }

        private void FixedUpdate()
        {
            Vector2 wantedVelocity = (_currentGrabPosition - _tip.position).normalized * _speed;
            Vector2 acceleration = wantedVelocity - _tip.velocity;
            acceleration = Vector2.ClampMagnitude(acceleration, 1f) * _acceleration;

            _tip.AddForce(acceleration * _tip.mass);
        }

        private void OnDestroy()
        {
            _seeker.OnDestroy();
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
            
            Handles.color = Color.yellow;
            Handles.DrawSolidDisc(GetIdealGrabPosition(), Vector3.forward, 0.1f);
            
            Handles.color = Color.blue;
            Handles.DrawSolidDisc(_currentGrabPosition, Vector3.forward, 0.1f);
            Handles.DrawLine(_tip.position, _currentGrabPosition);
#endif
        }

        private IEnumerator UpdateGrabPositionRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(UPDATE_GRAB_POSITION_STEP);
            
            while (true)
            {
                UpdateGrabPosition();
                yield return wait;
            }
        }

        [ContextMenu(nameof(UpdateGrabPosition))]
        private void UpdateGrabPosition()
        {
            _seeker.StartPath(_transform.position, GetIdealGrabPosition(), OnPathCalculated);
        }

        private void OnPathCalculated(Path path)
        {
            _currentGrabPosition = path.vectorPath.Last();
        }

        private Vector3 GetIdealGrabPosition()
        {
            return _transform.position + _transform.up * _radius;
        }
    }
}