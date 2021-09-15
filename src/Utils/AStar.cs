using System;
using System.Collections.Generic;
using System.Linq;

using Godot;


namespace TowerDefenseMC.Utils
{
    public class AStar
    {
        private const int OrthogonalMovementCost = 10;
        private const int DiagonalMovementCost = 14;
        
        private readonly List<Vector2> _openTiles;
        private readonly List<Vector2> _closedTiles;
        private readonly Dictionary<Vector2, int> _gScores;
        private readonly Dictionary<Vector2, int> _hScores;
        private readonly Dictionary<Vector2, Vector2> _cameFrom;

        private readonly bool _isDiagonalMovementAllowed;
        
        public AStar(bool isDiagonalMovementAllowed)
        {
            _openTiles = new List<Vector2>();
            _closedTiles = new List<Vector2>();
            _gScores = new Dictionary<Vector2,int>();
            _hScores = new Dictionary<Vector2,int>();
            _cameFrom = new Dictionary<Vector2, Vector2>();
            _isDiagonalMovementAllowed = isDiagonalMovementAllowed;
        }
        public AStar()
        {
            _openTiles = new List<Vector2>();
            _closedTiles = new List<Vector2>();
            _gScores = new Dictionary<Vector2,int>();
            _hScores = new Dictionary<Vector2,int>();
            _cameFrom = new Dictionary<Vector2, Vector2>();
            _isDiagonalMovementAllowed = false;
        }
        
        public List<Vector2> FindPath(Vector2 start, Vector2 goal)
        {
            _gScores.Add(start, 0);
            _hScores.Add(start, CalculateHeuristicScore(start, goal));
            
            _openTiles.Add(start);

            while (_openTiles.Count != 0)
            {
                Vector2 current = FindLowestFScore();
                
                _openTiles.Remove(current);
                _closedTiles.Add(current);

                if (current == goal)
                {
                    return ReconstructPath(current);
                }

                List<Vector2> adjTiles = GetAdjacentNodes(current);

                foreach (Vector2 adj in adjTiles)
                {
                    int gScoreTentatives = CalculateGScore(adj, current);
                    
                    if (_closedTiles.Contains(adj)) continue;

                    _gScores.TryGetValue(adj, out int gScoreValue);

                    if (_openTiles.Contains(adj) && gScoreTentatives >= gScoreValue) continue;

                    _cameFrom.Add(adj, current);
                    _gScores.Add(adj, gScoreTentatives);
                    _hScores.Add(adj, CalculateHeuristicScore(adj, goal));

                    if (!_openTiles.Contains(adj))
                    {
                        _openTiles.Add(adj);
                    }
                }
            }

            return null;
        }

        private Vector2 FindLowestFScore()
        {
            int lowestScore = Int32.MaxValue;
            Vector2 lowestScoreVector = _openTiles.Last();

            foreach (Vector2 vec in _openTiles)
            {
                _gScores.TryGetValue(vec, out int gScore);
                _hScores.TryGetValue(vec, out int hScore);

                int fScore = gScore + hScore;

                if (fScore >= lowestScore) continue;

                lowestScore = fScore;
                lowestScoreVector = vec;
            }

            return lowestScoreVector;
        }
        
        private int CalculateGScore(Vector2 current, Vector2 previous)
        {
            _gScores.TryGetValue(previous, out int prevScore);

            return prevScore + (IsDiagonalMovement(current, previous) ? DiagonalMovementCost : OrthogonalMovementCost);
        }

        private static bool IsDiagonalMovement(Vector2 current, Vector2 previous)
        {
            return previous == new Vector2(current.x + 1, current.y + 1) ||
                   previous == new Vector2(current.x + 1, current.y - 1) ||
                   previous == new Vector2(current.x - 1, current.y + 1) ||
                   previous == new Vector2(current.x - 1, current.y - 1);
        }


        private static int CalculateHeuristicScore(Vector2 current, Vector2 goal) {
            int dx = Math.Abs((int) (current.x - goal.x));
            int dy = Math.Abs((int) (current.y - goal.y));
            int diagonal, orthogonal;
            
            if(dx > dy) 
            {
                diagonal = dy;
                orthogonal = dx - diagonal;
            }
            else 
            {
                diagonal = dx;
                orthogonal = dy - diagonal;
            }
            
            return diagonal * DiagonalMovementCost + orthogonal * OrthogonalMovementCost;
        }

        
        private List<Vector2> ReconstructPath(Vector2 current) {
            if (!_cameFrom.TryGetValue(current, out Vector2 prev)) return new List<Vector2>();

            List<Vector2> path = ReconstructPath(prev);
            path.Add(current);

            return path;
        }
        
        private List<Vector2> GetAdjacentNodes(Vector2 current) 
        {
            List<Vector2> adj = new List<Vector2>
            {
                new Vector2(current.x - 1, current.y),
                new Vector2(current.x + 1, current.y),
                new Vector2(current.x, current.y - 1),
                new Vector2(current.x, current.y + 1)
            };

            if (!_isDiagonalMovementAllowed) return adj;

            adj.Add(new Vector2(current.x + 1, current.y + 1));
            adj.Add(new Vector2(current.x + 1, current.y - 1));
            adj.Add(new Vector2(current.x - 1, current.y + 1));
            adj.Add(new Vector2(current.x - 1, current.y - 1));

            return adj;
        }

        public void Clear()
        {
            _openTiles.Clear();
            _closedTiles.Clear();
            _gScores.Clear();
            _hScores.Clear();
            _cameFrom.Clear();
        }
    }
}