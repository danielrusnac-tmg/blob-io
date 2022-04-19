using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class BetterBlobBody : MonoBehaviour
    {
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _resolution = 30f;

        private void Awake()
        {
            int pointCount = GetPointCount();
        }

        private void Update()
        {
            UpdateBody();
        }

        [ContextMenu(nameof(UpdateBody))]
        private void UpdateBody()
        {
        }

        private int GetPointCount()
        {
            return (int)(2 * Mathf.PI * _radius / (1 / _resolution));
        }
    }
}