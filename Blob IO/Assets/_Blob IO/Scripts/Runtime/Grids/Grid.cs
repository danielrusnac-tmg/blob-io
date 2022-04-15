using System;
using UnityEngine;

namespace BlobIO.Grids
{
    public class Grid<T>
    {
        public event Action<Vector2Int> CellChanged; 
        public readonly int Width;
        public readonly int Height;

        private readonly float _cellSize;
        private readonly Vector2 _offset;
        private readonly T[,] _gridCells;

        public Grid(int width, int height, float cellSize, Vector2 center)
        {
            _offset = center - new Vector2(width, height) * cellSize * 0.5f;
            Width = width;
            Height = height;
            _cellSize = cellSize;
            _gridCells = new T[width, height];
        }

        public Vector2 GetWorldPosition(Vector2Int coordinate)
        {
            return _offset + (Vector2)coordinate * _cellSize;
        }

        public Vector2Int GetCoordinate(Vector2 worldPosition)
        {
            return new Vector2Int((int) (worldPosition.x / _cellSize), (int) (worldPosition.y / _cellSize));
        }

        public void SetValue(T value, Vector2 worldPosition)
        {
            SetValue(value, GetCoordinate(worldPosition));
        }
        
        public void SetValue(T value, Vector2Int coordinate)
        {
            _gridCells[coordinate.x, coordinate.y] = value;
            CellChanged?.Invoke(coordinate);
        }

        public T GetValue(Vector2 worldPosition)
        {
            return GetValue(GetCoordinate(worldPosition));
        }

        public T GetValue(Vector2Int coordinate)
        {
            return _gridCells[coordinate.x, coordinate.y];
        }
    }
}