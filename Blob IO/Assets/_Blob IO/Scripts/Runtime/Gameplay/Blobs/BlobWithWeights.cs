using System.Collections.Generic;
using BlobIO.Blobs.Tentacles;
using BlobIO.Controllers;
using UnityEngine;

namespace BlobIO.Blobs
{
    public class BlobWithWeights : MonoBehaviour, IControllable
    {
        [SerializeField] private float _tentacleCreationTime = 0.3f;
        [Range(1, 36)]
        [SerializeField] private int _tentacleCount = 8;
        [Range(0f, 360f)]
        [SerializeField] private float _angleSpan = 160f;
        [SerializeField] private float _baseRadius = 0.5f;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private AnimationCurve _inputDistribution;
        [SerializeField] private AnimationCurve _wightDistribution;
        [SerializeField] private Rigidbody2D _blobRigidbody;
        [SerializeField] private PersistentTentacle _tentaclePrefab;
        [SerializeField] private PersistentTentacleSetting[] _tentacleSettings;

        private float _createTentacleCooldown;
        private List<PersistentTentacle> _tentacles = new List<PersistentTentacle>();

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
            GenerateTentacles();
            RemoveExtraTentacles();

            if (_input.IsMoving)
            {
                UpdateTentaclePositions();
                CountGrabbingTentacles();
                GrabOntoWalls();
            }

            RemoveInvalidTentacles();
            UpdateWeights();

            _createTentacleCooldown += Time.deltaTime;
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

            float dot = Vector2.Dot(tentacle.GetTentacleDirection, _input.MoveDirection.normalized);
            
            return _wightDistribution.Evaluate((dot + 1) / 2);
        }

        public void CountGrabbingTentacles()
        {
            ActiveTentaclePercent = 0f;
            AverageTentacleStretchiness = 0f;
            MidPoint = Vector2.zero;
            
            foreach (PersistentTentacle tentacle in _tentacles)
            {
                if (tentacle.IsGrabbing && tentacle.CanApplyForce)
                {
                    ActiveTentaclePercent++;
                    AverageTentacleStretchiness += tentacle.Stretchiness;
                    MidPoint += tentacle.TipPosition;
                }
            }

            ActiveTentaclePercent /= _tentacleCount;
            AverageTentacleStretchiness /= _tentacleCount;
            MidPoint /= _tentacleCount;
        }

        public void GenerateTentacles()
        {
            while (_tentacles.Count < _tentacleCount)
            {
                PersistentTentacle tentacle = Instantiate(_tentaclePrefab, transform);
                tentacle.Construct(_blobPoint, _tentacleSettings[Random.Range(0, _tentacleSettings.Length)]);
                _tentacles.Add(tentacle);
            }
        }

        public void RemoveExtraTentacles()
        {
            while (_tentacles.Count > _tentacleCount)
            {
                Destroy(_tentacles[0].gameObject);
                _tentacles.RemoveAt(0);
            }
        }

        public void UpdateTentaclePositions()
        {
            float angleStep = _angleSpan / _tentacleCount;
            Vector2 center = transform.position;
            Quaternion startRotation = Quaternion.AngleAxis(_angleSpan / 2, Vector3.forward);

            for (int i = 0; i < _tentacles.Count; i++)
            {
                Quaternion rotation = Quaternion.AngleAxis(angleStep * (i + 0.5f), Vector3.forward) * startRotation;
                Vector2 direction = rotation * Vector3.up;
                Vector2 position = center + direction * _baseRadius;
                
                _tentacles[i].transform.SetPositionAndRotation(position, rotation);
                // _tentacles[i].UpdateTentacle();
            }
        }

        private void RemoveInvalidTentacles()
        {
            foreach (PersistentTentacle tentacle in _tentacles)
            {
                if (tentacle.ShouldRelease())
                {
                    tentacle.Release();
                }
            }
        }

        private void GrabOntoWalls()
        {
            if (_createTentacleCooldown < _tentacleCreationTime)
                return;
            
            foreach (PersistentTentacle tentacle in _tentacles)
            {
                if (tentacle.IsFacingWall(out Vector2 point, out GameObject grabbedObject) && tentacle.ShouldGrab())
                {
                    tentacle.Grab(point, grabbedObject);
                    _createTentacleCooldown = 0f;
                    break;
                }
            }
        }

        private void UpdateWeights()
        {
            foreach (PersistentTentacle tentacle in _tentacles)
                tentacle.SetWeight(GetTentacleWeight(tentacle));
        }
    }
}