
using System.Collections.Generic;
using System.Threading.Tasks;

using Godot;

using TowerDefenseMC.Enemies;
using TowerDefenseMC.Singletons;
using TowerDefenseMC.Towers;
using TowerDefenseMC.Towers.Projectiles;
using TowerDefenseMC.UserInterface.TopBar;
using TowerDefenseMC.UserInterface.PauseMenu;

using AStar = TowerDefenseMC.Utils.AStar;


namespace TowerDefenseMC.Levels
{
    public class LevelTemplate : Node2D
    {
        public TileMap TileMap { get; private set; }

        public Player Player;
        private EnemySpawner _enemySpawner;
        private TopBar _topBar;
        private BuildTool _buildTool;
        private ProjectileSpawner _projectileSpawner;
        private PauseMenu _pauseMenu;
        private Timer _waveTimer;


        private Dictionary<Vector2, TowerTemplate> _tilesWithTowers;
        private List<List<Vector2>> _paths;
        private List<List<Vector2>> _rivers;

        public override void _Ready()
        {
            TileMap = GetNode<TileMap>("TileMap");
            _topBar = GetNode<TopBar>("UI/TopBar");
            _pauseMenu = GetNode<PauseMenu>("Pause/PauseMenu");
            _waveTimer = GetNode<Timer>("WaveTimer");

            _tilesWithTowers = new Dictionary<Vector2, TowerTemplate>();
        }

        public async void PreStart(int level)
        {
            LevelDataReader ldr = GetNode<LevelDataReader>("/root/LevelDataReader");
            LevelData levelData = ldr.GetLevelData(level);

            _paths = CalculateTilesInPointsLists(levelData.EnemyPathsPoints);
            _rivers = CalculateTilesInPointsLists(levelData.RiversPoints);
            
            TerrainBuilder terrainBuilder = new TerrainBuilder(this, levelData.IsSnowy, TileMap);
            terrainBuilder.FillViewPortWithTile(GetViewSize());
            
            Task customTilesTask = terrainBuilder.DrawCustomTiles(levelData.Tiles);
            Task pathAndRiversTask = terrainBuilder.DrawPathsAndRivers(_paths, _rivers);

            Player = new Player(levelData.StartHealth, levelData.StartCrystals);
            _enemySpawner = new EnemySpawner(this, _paths, _waveTimer, levelData.Waves);

            _topBar.Crystals.SetPlayer(Player);
            _topBar.HealthBar.SetPlayer(Player);
            _topBar.WaveTimer.SetWaveTimer(_waveTimer);
            
            await Task.WhenAll(customTilesTask, pathAndRiversTask);
        }

        public void Start()
        {
            _buildTool = new BuildTool(this);
            _projectileSpawner = new ProjectileSpawner(this);
            
            _enemySpawner.StartNextWaveTimer();
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

        public void AddTowerOnTile(Vector2 tile, TowerTemplate tower)
        {
            _tilesWithTowers.Add(tile, tower);
        }

        public bool TileHasTower(Vector2 tile)
        {
            return _tilesWithTowers.ContainsKey(tile);
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

        public HashSet<TowerTemplate> GetTowersOnArea(Vector2 center, int range)
        {
            HashSet<TowerTemplate> towersOnArea = new HashSet<TowerTemplate>();
            
            HashSet<Vector2> tilesOnArea = GetTilesOnArea(center, range);

            foreach (Vector2 tile in tilesOnArea)
            {
                if (_tilesWithTowers.TryGetValue(tile, out TowerTemplate tower))
                {
                    towersOnArea.Add(tower);
                }
            }

            return towersOnArea;
        }

        public HashSet<Vector2> GetTilesOnArea(Vector2 center, int range)
        {
            HashSet<Vector2> tilesOnArea = new HashSet<Vector2>();

            for (int x = 0; x <= range; x++)
            {
                for (int y = 0; y <= range; y++)
                {
                    tilesOnArea.Add(new Vector2(center.x + x, center.y + y));
                    tilesOnArea.Add(new Vector2(center.x + x, center.y - y));
                    tilesOnArea.Add(new Vector2(center.x - x, center.y + y));
                    tilesOnArea.Add(new Vector2(center.x - x, center.y - y));
                }
            }

            return tilesOnArea;
        }
        
        public void SpawnProjectile(PackedScene projectile, Vector2 pos, PhysicsBody2D target, int damage, float projectileSpeed)
        {
            _projectileSpawner.SpawnProjectile(projectile, pos, target, damage, projectileSpeed);
        }

        public void OnSelectTowerButtonDown(string towerName, int towerCost)
        {
            _buildTool.OnSelectTowerButtonDown(towerName, towerCost);
        }

        public void OnTowerButtonMouseEntered()
        {
            _buildTool.OnTowerButtonMouseEntered();
        }

        public void OnTowerButtonMouseExited()
        {
            _buildTool.OnTowerButtonMouseExited();
        }

        public void OnTouchScreenButtonReleased(string towerName, TowerTemplate tower)
        {
            _buildTool.TowerStatistics(towerName, tower);
        }

        public void OnPauseMenuButtonPressed()
        {
            _pauseMenu.SetPauseMode(true);
        }

        public void OnWaveTimerTimeout()
        {
            _enemySpawner.SpawnWaveEnemies();
        }

        public void OnEnemyReachEndOfPath(int damage)
        {
            Player.TakeDamage(damage);
        }

        public void OnEnemyDestroyed(int feed)
        {
            Player.Crystals += feed;
        }
    }
}