using UnityEngine;

namespace BlobIO.SmartBlobs
{
    [CreateAssetMenu(fileName = "New Tentacle Setting", menuName = CreationPaths.CREATE + "Tentacle Setting")]
    public class SmartTentacleSetting : ScriptableObject
    {
        [SerializeField] private LayerMask _grabMask;
        [SerializeField] private float _radius = 8f;
        [SerializeField] private float _stepDistance = 4f;
        [SerializeField] private float _grabRayRadius = 0.5f;
        
        public LayerMask GrabMask => _grabMask;
        public float Radius => _radius;
        public float StepDistance => _stepDistance;
        public float GrabRayRadius => _grabRayRadius;
    }
}