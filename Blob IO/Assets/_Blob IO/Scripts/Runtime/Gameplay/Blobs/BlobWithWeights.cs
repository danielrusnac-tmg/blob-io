using System;
using BlobIO.Blobs.Tentacles;
using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.Blobs
{
    public class BlobWithWeights : MonoBehaviour, IControllable
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private AnimationCurve _inputDistribution;
        [SerializeField] private AnimationCurve _wightDistribution;
        [SerializeField] private Rigidbody2D _blobRigidbody;
        [SerializeField] private PersistentTentacle[] _tentacles;

        private TentaclePoint _blobPoint;
        private IControllableInput _input;
        public float ActiveTentaclePercent { get; private set; }
        public float AverageTentacleStretchiness { get; private set; }
        public Vector2 MidPoint { get; private set; }

        private void Awake()
        {
            _blobPoint = new TentaclePoint(gameObject, transform.position);

            ConstructTentacles();
        }

        private void Update()
        {
            CountGrabbingTentacles();
            
            foreach (PersistentTentacle tentacle in _tentacles)
            {
                if (tentacle.ShouldRelease())
                {
                    tentacle.Release();
                }

                if (_input.IsMoving && tentacle.IsFacingWall(out Vector2 point, out GameObject grabbedObject) && tentacle.ShouldGrab())
                {
                    tentacle.Grab(point, grabbedObject);
                }
                
                tentacle.SetWeight(GetTentacleWeight(tentacle));
            }
        }
        
        private void FixedUpdate()
        {
            if (_input.IsMoving)
            {
                Vector2 wantedVelocity = _input.MoveDirection * _speed;
                Vector2 acceleration = wantedVelocity - _blobRigidbody.velocity;
                
                _blobRigidbody.AddForce(acceleration * _inputDistribution.Evaluate(ActiveTentaclePercent), ForceMode2D.Impulse);
            }
        }

        public void SetInput(IControllableInput input)
        {
            _input = input;
        }

        private void ConstructTentacles()
        {
            foreach (PersistentTentacle tentacle in _tentacles)
                tentacle.Construct(_blobPoint);
        }

        private float GetTentacleWeight(PersistentTentacle tentacle)
        {
            if (!_input.IsMoving)
                return 1f;

            float dot = Vector2.Dot(tentacle.GetTentacleDirection, _input.MoveDirection);
            
            return _wightDistribution.Evaluate((dot + 1) / 2);
        }
        
        public void CountGrabbingTentacles()
        {
            ActiveTentaclePercent = 0f;
            AverageTentacleStretchiness = 0f;
            MidPoint = Vector2.zero;
            
            foreach (PersistentTentacle tentacle in _tentacles)
            {
                if (tentacle.IsGrabbing)
                {
                    ActiveTentaclePercent++;
                    AverageTentacleStretchiness += tentacle.Stretchiness;
                    MidPoint += tentacle.TipPosition;
                }
            }

            ActiveTentaclePercent /= _tentacles.Length;
            AverageTentacleStretchiness /= _tentacles.Length;
            MidPoint /= _tentacles.Length;
        }

    }
}