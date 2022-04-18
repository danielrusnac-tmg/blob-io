using System.Collections.Generic;
using UnityEngine;

namespace BlobIO.Blobs.Tentacles
{
    public class TentacleBrain : MonoBehaviour
    {
        [Range(1, 16)]
        [SerializeField] private int _tentacleCount = 8;
        [Range(0f, 360f)]
        [SerializeField] private float _angleSpan = 160f;
        [SerializeField] private float _baseRadius = 0.5f;
        [SerializeField] private PersistentTentacle _tentaclePrefab;

        private Quaternion _lookRotation;
        private TentaclePoint _centerPoint;
        private List<PersistentTentacle> _tentacles = new List<PersistentTentacle>();

        public float ActiveTentaclePercent { get; private set; }
        public float AverageTentacleStretchiness { get; private set; }
        public Vector2 MidPoint { get; private set; }

        private void Awake()
        {
            _centerPoint = new TentaclePoint(gameObject, transform.position);
            Look(Vector2.up);
        }

        private void Update()
        {
            RemoveExtraTentacles();
            GenerateTentacles();
            UpdateTentaclePositions();
            CountGrabbingTentacles();
        }

        public void Look(Vector2 direction)
        {
            _lookRotation = Quaternion.LookRotation(direction, Vector3.forward);
        }

        private void CountGrabbingTentacles()
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

            ActiveTentaclePercent /= _tentacleCount;
            AverageTentacleStretchiness /= _tentacleCount;
            MidPoint /= _tentacleCount;
        }

        private void UpdateTentaclePositions()
        {
            float angleStep = _angleSpan / _tentacleCount;
            Vector2 center = transform.position;
            Quaternion startRotation = _lookRotation * Quaternion.AngleAxis(_angleSpan / 2, Vector3.forward);

            for (int i = 0; i < _tentacles.Count; i++)
            {
                Quaternion rotation = Quaternion.AngleAxis(angleStep * (i + 0.5f), Vector3.forward) * startRotation;
                Vector2 direction = rotation * Vector3.up;
                Vector2 position = center + direction * _baseRadius;
                
                _tentacles[i].transform.SetPositionAndRotation(position, rotation);
            }
        }

        private void GenerateTentacles()
        {
            while (_tentacles.Count < _tentacleCount)
            {
                PersistentTentacle tentacle = Instantiate(_tentaclePrefab, transform);
                tentacle.Construct(_centerPoint);
                _tentacles.Add(tentacle);
            }
        }

        private void RemoveExtraTentacles()
        {
            while (_tentacles.Count > _tentacleCount)
            {
                Destroy(_tentacles[0].gameObject);
                _tentacles.RemoveAt(0);
            }
        }
    }
}