using UnityEngine;

namespace BlobIO.Grids
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = CreationPaths.LEVELS + "Level Data")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private byte[,] _cells;

        public void Save(byte[,] cells)
        {
            _cells = cells;
        }

        public byte[,] Load()
        {
            return _cells;
        }
    }
}