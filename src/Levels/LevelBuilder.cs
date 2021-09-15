
using System.Collections.Generic;

using Godot;

using AStar = TowerDefenseMC.Utils.AStar;
using TileMap = Godot.TileMap;


namespace TowerDefenseMC.Levels
{
    public class LevelBuilder : Node2D
    {
        private TileMap _tileMap;

        private TerrainBuilder _terrainBuilder;

        private List<List<Vector2>> _paths;
        private List<List<Vector2>> _rivers;

        public override void _Ready()
        {
            _tileMap = GetNode<TileMap>("TileMap");
            
            _paths = new List<List<Vector2>>();

            LevelDataReader ldr = GetNode<LevelDataReader>("/root/LevelDataReader");
            LevelData levelData = ldr.GetLevelData(1);
            
            _paths = CalculateTilesInPointsLists(levelData.EnemyPathsPoints);
            _rivers = CalculateTilesInPointsLists(levelData.RiversPoints);

            _terrainBuilder = new TerrainBuilder(levelData.IsSnowy, _tileMap);
            _terrainBuilder.FillViewPortWithTile(GetViewSize());
            _terrainBuilder.DrawCustomTiles(levelData.Tiles);
            _terrainBuilder.DrawPathsAndRivers(_paths, _rivers);
        }

        private Vector2 GetViewSize()
        {
            Transform2D canvasTransform = GetCanvasTransform();
            return GetViewportRect().Size / canvasTransform.Scale;
        }

        private List<List<Vector2>> CalculateTilesInPointsLists(List<List<Vector2>> pointsLists)
        {
            List<List<Vector2>> listsOfTiles = new List<List<Vector2>>();
            
            AStar aStar = new AStar(false);
            
            foreach (List<Vector2> list in pointsLists)
            {
                List<Vector2> tiles = new List<Vector2>();

                for (int i = 1; i < list.Count; i++)
                {
                    tiles.AddRange(aStar.FindPath(list[i - 1], list[i]));
                    aStar.Clear();
                }

                listsOfTiles.Add(tiles);
            }

            return listsOfTiles;
        }

    }
}