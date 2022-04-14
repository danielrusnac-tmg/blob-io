using Cinemachine;
using UnityEngine;

namespace BlobIO.Gameplay.Cameras
{
    public class GameplayCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        public void SetTarget(Transform target)
        {
            _virtualCamera.m_Follow = target;
            _virtualCamera.m_LookAt = target;
        }
    }
}