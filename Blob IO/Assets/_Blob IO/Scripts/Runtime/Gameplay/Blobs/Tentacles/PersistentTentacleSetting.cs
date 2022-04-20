using UnityEngine;

namespace BlobIO.Blobs.Tentacles
{
    [CreateAssetMenu(fileName = "New Persistent Tentacle Setting", menuName = CreationPaths.CREATE + "Persistent Tentacle Setting")]
    public class PersistentTentacleSetting : ScriptableObject
    {
        [SerializeField] private LayerMask _grabMask;
        [SerializeField] private LayerMask _solidMask;
        [SerializeField] private float _checkRadius = 8f;
        [SerializeField] private float _tentacleRadius = 4f;
        [SerializeField] private float _stepDistance = 3f;
        [SerializeField] private float _releaseDistance = 8f;

        public LayerMask SolidMask => _solidMask;
        public LayerMask GrabMask => _grabMask;
        public float CheckRadius => _checkRadius;
        public float TentacleRadius => _tentacleRadius;
        public float StepDistance => _stepDistance;
        public float ReleaseDistance => _releaseDistance;
    }
}