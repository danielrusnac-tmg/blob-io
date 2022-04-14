using UnityEditor;
using UnityEngine;

namespace BlobIO.Gameplay
{
    public class SmartBlob : MonoBehaviour
    {
        [SerializeField] private LayerMask _wallMask;
        [SerializeField] private float _radius = 8f;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition - Vector3.forward * _mainCamera.transform.position.z);
                Vector2 mouseDirection = mouseWorldPosition - transform.position;
                
                Debug.DrawRay(transform.position, mouseDirection, Color.green, Time.deltaTime);
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
#endif
        }
    }
}