using UnityEngine;

namespace BlobIO.Gameplay
{
    [CreateAssetMenu(fileName = "New Global Settings", menuName = CreationPaths.CREATE + "Global Settings")]
    public class GlobalSettings : ScriptableObject
    {
        [SerializeField] private PlayerType _playerType;
        [SerializeField] private LayerMask _wallMask;

        public PlayerType PlayerType => _playerType;
        public LayerMask WallMask => _wallMask;
    }
}