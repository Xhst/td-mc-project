using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Singletons;
using TowerDefenseMC.Utils;


namespace TowerDefenseMC.Enemies
{
    public class EnemySpawner
    {
        private readonly LevelTemplate _levelTemplate;
        private readonly List<List<Vector2>> _paths;

        private readonly Dictionary<string, EnemyData> _enemiesData;
        
        public EnemySpawner(LevelTemplate levelTemplate, List<List<Vector2>> paths)
        {
            _levelTemplate = levelTemplate;
            _paths = paths;

            EnemyDataReader edr = _levelTemplate.GetNode<EnemyDataReader>("/root/EnemyDataReader");
            _enemiesData = edr.GetEnemiesData();
        }
        
        public async void SpawnEnemies()
        {
            PackedScene enemyScene = ResourceLoader.Load<PackedScene>("res://scenes/enemies/EnemyTemplate.tscn");
        
            Node2D pathsContainer = _levelTemplate.GetNode<Node2D>("Paths");
            YSort enemyContainer = _levelTemplate.GetNode<YSort>("EntitiesContainer");

            foreach (List<Vector2> path in _paths)
            {
                Path2D path2D = new Path2D();
                pathsContainer.AddChild(path2D);
                
                Curve2D enemyCurve = new Curve2D();
                
                foreach (Vector2 tile in path)
                {
                    enemyCurve.AddPoint(tile.CartesianToIsometric());
                }

                path2D.Curve = enemyCurve;

                for (int i = 0; i < 50; i++)
                {
                    EnemyTemplate enemy = (EnemyTemplate) enemyScene.Instance();
                    enemyContainer.AddChild(enemy);

                    PathFollow2D pathFollow = new PathFollow2D { Rotate = false, Loop = false };

                    RemoteTransform2D remoteTransform = new RemoteTransform2D { RemotePath = enemy.GetPath() };

                    enemy.PositioningNode = pathFollow;

                    pathFollow.AddChild(remoteTransform);
                    path2D.AddChild(pathFollow);

                    await _levelTemplate.ToSignal(_levelTemplate.GetTree().CreateTimer(0.5f), "timeout");
                }
            }
        }
    }
}