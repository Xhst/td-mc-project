
using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Towers;

using AStar = TowerDefenseMC.Utils.AStar;
using TileMap = Godot.TileMap;


namespace TowerDefenseMC.Levels
{
    public class LevelTemplate : Node2D
    {
        public TileMap TileMap { get; private set; }
        
        private BuildTool _buildTool;
        private ProjectileSpawner _projectileSpawner;

        private List<List<Vector2>> _paths;
        private List<List<Vector2>> _rivers;

        public override void _Ready()
        {
            TileMap = GetNode<TileMap>("TileMap");

            LevelDataReader ldr = GetNode<LevelDataReader>("/root/LevelDataReader");
            LevelData levelData = ldr.GetLevelData(1);

            _paths = CalculateTilesInPointsLists(levelData.EnemyPathsPoints);
            _rivers = CalculateTilesInPointsLists(levelData.RiversPoints);
            
            TerrainBuilder terrainBuilder = new TerrainBuilder(levelData.IsSnowy, TileMap);
            terrainBuilder.FillViewPortWithTile(GetViewSize());
            terrainBuilder.DrawCustomTiles(levelData.Tiles);
            terrainBuilder.DrawPathsAndRivers(_paths, _rivers);

            _buildTool = new BuildTool(this);
            _projectileSpawner = new ProjectileSpawner(this);

            new EnemySpawner(this, _paths).SpawnEnemies();

            Singletons.Globals.MainGameNode = this;
        }

        private Vector2 GetViewSize()
        {
            Transform2D canvasTransform = GetCanvasTransform();
            return GetViewportRect().Size / canvasTransform.Scale;
        }

        public override void _PhysicsProcess(float delta)
        {
            _buildTool.Process();
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

        public void SpawnProjectile(PackedScene projectile, Vector2 pos, PhysicsBody2D target)
        {
            _projectileSpawner.SpawnProjectile(projectile, pos, target);
        }

        public void OnSelectTowerButtonDown(string towerName)
        {
            _buildTool.OnSelectTowerButtonDown(towerName);
        }

        public void OnTowerButtonMouseEntered()
        {
            _buildTool.OnTowerButtonMouseEntered();
        }

        public void OnTowerButtonMouseExited()
        {
            _buildTool.OnTowerButtonMouseExited();
        }

    }
}