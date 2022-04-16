using System;
using System.Collections.Generic;
using BlobIO.Levels.Grids;
using UnityEngine;

namespace BlobIO.Services.Pathfinding
{
    public class Path
    {
        public List<PathCell> Cells;

        public Path()
        {
            Cells = new List<PathCell>();
        }
    }

    public class PathCell
    {
        public float G;
        public float H;
        public float F;
        public PathCell PreviousCell;
    }

    public class AStartPathfinder : IPathfinder
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;
        
        private Grid<PathCell> _grid;

        public AStartPathfinder()
        {
            _grid = new Grid<PathCell>(50, 30, 0.5f, Vector2.zero);
        }

        public Path FindPath(Vector2Int start, Vector2Int end)
        {
            PathCell startCell = _grid.GetCell(start);
            List<PathCell> openCells = new List<PathCell>() {startCell};
            List<PathCell> closedCells = new List<PathCell>();
            
            for (var x = 0; x < _grid.Width; x++)
            {
                for (var y = 0; y < _grid.Height; y++)
                {
                    PathCell cell = _grid.GetCell(new Vector2Int(x, y));
                    cell.G = Mathf.Infinity;
                    cell.F = cell.G + cell.H;
                    cell.PreviousCell = null;
                }
            }

            startCell.F = 0;
            // startCell.G = CalculateCost();

            return new Path();
        }

        private float CalculateCost(Vector2Int a, Vector2Int b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int rawDistance = Mathf.Abs(xDistance - yDistance);

            return rawDistance;
        }
    }
}