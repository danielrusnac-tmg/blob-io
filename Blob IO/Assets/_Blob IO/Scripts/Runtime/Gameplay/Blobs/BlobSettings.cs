using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlobIO.Blobs
{
    [CreateAssetMenu(fileName = "New Blob Setting", menuName = CreationPaths.CREATE + "Blob Settings")]
    public class BlobSettings : ScriptableObject
    {
        [SerializeField] private float _moveForce = 30f;
        [SerializeField] private float _blobRadius;
        [SerializeField] private float _forceVerticalOffset = 1.2f;
        [SerializeField] private AnimationCurve _angleBias = AnimationCurve.Constant(0f, 1f, 1f);

        [Header("Tentacles")]
        [SerializeField] private float _updateTentaclesDelay = 0.2f;
        [SerializeField] private int _wantedTentacleCount = 7;
        [SerializeField] private float _desireThreshold;
        [SerializeField] private float _removeDesireThreshold = -1f;
        [SerializeField] private float _radius = 8f;
        [SerializeField] private float _stiffness = 100f;
        [SerializeField] private float _damp = 2f;

        public float MoveForce => _moveForce;
        public float ForceVerticalOffset => _forceVerticalOffset;
        public float BlobRadius => _blobRadius;
        public float UpdateTentaclesDelay => _updateTentaclesDelay;
        public int WantedTentacleCount => _wantedTentacleCount;
        public float DesireThreshold => _desireThreshold;
        public float RemoveDesireThreshold => _removeDesireThreshold;
        public float Radius => _radius;
        public float Stiffness => _stiffness;
        public float Damp => _damp;

        public float GetRandomAngleOffset()
        {
            return _angleBias.Evaluate(Random.value) * 180f;
        }
    }
}