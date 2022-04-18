using System;
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

        private Vector2 _lookDirection;
        private Quaternion _lookRotation;
        private List<PersistentTentacle> _tentacles = new List<PersistentTentacle>();

        private void Awake()
        {
            Look(Vector2.up);
        }

        private void Update()
        {
            RefreshTentacles();
        }

        public void Look(Vector2 direction)
        {
            _lookDirection = direction;
            _lookRotation = Quaternion.LookRotation(direction, Vector3.forward);
        }

        private void RefreshTentacles()
        {
            RemoveExtraTentacles();
            GenerateTentacles();
            UpdateTentaclePositions();
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
                _tentacles.Add(Instantiate(_tentaclePrefab, transform));
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