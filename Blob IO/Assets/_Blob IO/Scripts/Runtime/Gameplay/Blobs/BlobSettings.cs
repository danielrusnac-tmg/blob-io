using UnityEngine;

namespace BlobIO.Gameplay.Blobs
{
    [CreateAssetMenu(fileName = "New Blob Settings", menuName = CreationPaths.BLOBS + "Blob Settings")]
    public class BlobSettings : ScriptableObject
    {
        [SerializeField] private float _radius = 8f;
        [SerializeField] private float _desireThreshold;
        [SerializeField] private float _stiffness = 100f;
        [SerializeField] private float _damp = 2f;
        [SerializeField] private AnimationCurve _angleBias = AnimationCurve.Constant(0f, 1f, 1f);

        public float Radius => _radius;
        public float DesireThreshold => _desireThreshold;

        public float GetRandomAngleOffset()
        {
            return _angleBias.Evaluate(Random.value) * 180f;
        }

        public Spring CreateSpring()
        {
            return new Spring(_radius, _stiffness, _damp);
        }
    }
}