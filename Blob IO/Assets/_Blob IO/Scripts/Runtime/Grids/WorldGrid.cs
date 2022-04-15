using UnityEngine;

namespace BlobIO.Grids
{
    public class WorldGrid : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] private float _cellSize = 0.25f;
        [SerializeField] private Vector2Int _gridSize;

        private Grid<byte> _grid;

        private void OnValidate()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            _grid = new Grid<byte>(_gridSize.x, _gridSize.y, _cellSize, transform.position);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 1f, 1f, 0.17f);
            for (int x = 0; x < _grid.Width + 1; x++)
            {
                Gizmos.DrawLine(
                    _grid.GetWorldPosition(new Vector2Int(x, 0)),
                    _grid.GetWorldPosition(new Vector2Int(x, _grid.Height)));
            }
            
            for (int y = 0; y < _grid.Height + 1; y++)
            {
                Gizmos.DrawLine(
                    _grid.GetWorldPosition(new Vector2Int(0, y)),
                    _grid.GetWorldPosition(new Vector2Int(_grid.Width, y)));
            }
        }
    }
}