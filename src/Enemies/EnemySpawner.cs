using System.Collections.Generic;
using System.Threading.Tasks;

using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Singletons;
using TowerDefenseMC.Utils;


namespace TowerDefenseMC.Enemies
{
    public class EnemySpawner
    {
        private const float SpawnTimeBetweenEnemies = 0.5f;

        private readonly LevelTemplate _levelTemplate;
        private readonly List<List<Vector2>> _paths;

        private readonly Dictionary<string, EnemyData> _enemiesData;
        private readonly List<WaveData> _wavesData;

        private readonly PackedScene _enemyScene;
        private readonly YSort _enemyContainer;

        private readonly Timer _waveTimer;
        private int _currentWave = 0;
        
        public EnemySpawner(LevelTemplate levelTemplate, List<List<Vector2>> paths, Timer waveTimer, List<WaveData> wavesData)
        {
            _levelTemplate = levelTemplate;
            _paths = paths;
            _waveTimer = waveTimer;
            _wavesData = wavesData;

            _enemyScene = ResourceLoader.Load<PackedScene>("res://scenes/enemies/EnemyTemplate.tscn");
            _enemyContainer = _levelTemplate.GetNode<YSort>("EntitiesContainer");

            EnemyDataReader edr = _levelTemplate.GetNode<EnemyDataReader>("/root/EnemyDataReader");
            _enemiesData = edr.GetEnemiesData();
        }

        public void StartNextWaveTimer()
        {
            if (_wavesData.Count <= _currentWave)
            {
                _waveTimer.Stop();
                return;
            }
            
            GD.Print("Next wave");
            WaveData currentWaveData = _wavesData[_currentWave];
            
            _waveTimer.WaitTime = currentWaveData.WaitTime;
            _waveTimer.Start();
        }
        
        public void SpawnWaveEnemies()
        {
            foreach (List<Vector2> path in _paths)
            {
                Path2D path2D = GetPath2DWithCurveForPath(path);
                
                SpawnWaveForPath(_paths.IndexOf(path), path2D);
            }

            _currentWave++;
            StartNextWaveTimer();
        }

        private Path2D GetPath2DWithCurveForPath(List<Vector2> path)
        {
            Node2D pathsContainer = _levelTemplate.GetNode<Node2D>("Paths");
            
            Path2D path2D = new Path2D();
            pathsContainer.AddChild(path2D);
            Curve2D enemyCurve = new Curve2D();
                
            foreach (Vector2 tile in path)
            {
                enemyCurve.AddPoint(tile.CartesianToIsometric());
            }

            path2D.Curve = enemyCurve;

            return path2D;
        }

        private async void SpawnWaveForPath(int pathIndex, Path2D path2D)
        {
            if (_wavesData.Count <= _currentWave) return;
            
            WaveData currentWaveData = _wavesData[_currentWave];
            
            foreach (WaveEnemyGroupData enemyGroup in currentWaveData.WaveEnemies)
            {
                if (enemyGroup.Path != pathIndex) continue;
                        
                await SpawnEnemyGroup(enemyGroup.Name, enemyGroup.Amount, path2D);
            }
            
        }

        private async Task SpawnEnemyGroup(string enemyType, int amount, Path2D path2D)
        {
            _enemiesData.TryGetValue(enemyType, out EnemyData enemyData);

            for (int i = 0; i < amount; i++)
            {
                EnemyTemplate enemy = (EnemyTemplate) _enemyScene.Instance();
                _enemyContainer.AddChild(enemy);
                enemy.Init(enemyData);
                
                PathFollow2D pathFollow = new PathFollow2D { Rotate = false, Loop = false };

                RemoteTransform2D remoteTransform = new RemoteTransform2D { RemotePath = enemy.GetPath() };

                enemy.PositioningNode = pathFollow;

                pathFollow.AddChild(remoteTransform);
                path2D.AddChild(pathFollow);

                await _levelTemplate.ToSignal(_levelTemplate.GetTree().CreateTimer(SpawnTimeBetweenEnemies), "timeout");
            }
        }
        
    }
}