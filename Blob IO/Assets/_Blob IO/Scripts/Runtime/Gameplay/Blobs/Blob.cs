using System;
using BlobIO.Gameplay.Blobs.Controllers;
using UnityEditor;
using UnityEngine;

namespace BlobIO.Gameplay.Blobs
{
    public class Blob : MonoBehaviour, IControllable
    {
        [SerializeField] private LayerMask _wallMask;
        [SerializeField] private float _minRadius = 1.5f;
        [SerializeField] private float _maxRadius = 8f;
        [SerializeField] private float _sightAngle = 50f;

        private Vector2 _facingDirection = Vector2.right;
        private IControllableInput _input;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            ReadInput();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Vector3 center = transform.position;
            
            Handles.DrawWireDisc(center, Vector3.forward, _maxRadius);
            
            Handles.color = new Color(1f, 0.92f, 0.02f, 0.07f);
            Handles.DrawSolidDisc(center, Vector3.forward, _minRadius);
            Handles.DrawSolidArc(center, Vector3.forward, _facingDirection, _sightAngle, _maxRadius);
#endif
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }

        private bool HasAController()
        {
            return _input != null;
        }

        private void ReadInput()
        {
            if (!HasAController())
                return;
            
            _facingDirection = _input.Movement.normalized;
        }

        private void Move()
        {
        }
    }
}