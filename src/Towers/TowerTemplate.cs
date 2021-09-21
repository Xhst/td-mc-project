using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.Towers
{
    
    [Tool]
    public class TowerTemplate : Node2D
    {
        private bool _canShoot = true;

        private TowerData _towerData;

        [Signal]
        private delegate void ShootEvent(int damage);

        [Export]
        private PackedScene _projectile;
        
        private List<PhysicsBody2D> _targetList;

        public override void _Ready()
        {
            _targetList = new List<PhysicsBody2D>();
            SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
            
            //Connects the signal "ShootEvent" with the function "SpawnProjectile" passed by the Object "MainGameNode"
            Connect(nameof(ShootEvent), sceneManager.CurrentScene, nameof(LevelTemplate.SpawnProjectile)); 
        }

        public override void _PhysicsProcess(float delta)
        {
            if(_targetList.Count != 0 && _canShoot)
            {
                Shoot();
            }
        }

        private void SetAttackRange(int num)
        {
            _towerData.AttackRange = num;
            SetAttackRangeShape(num);
        }

        private void SetAttackRangeShape(int num)
        {
            ConvexPolygonShape2D newShape = new ConvexPolygonShape2D { Points = GetRangeShapePoints(num) };

            if (GetNode<CollisionShape2D>("AttackRange/AttackRangeCollision") != null)
            {
                GetNode<CollisionShape2D>("AttackRange/AttackRangeCollision").Shape = newShape;
            }
        }

        public static Vector2[] GetRangeShapePoints(int attackRange)
        {
            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(0, -32 - (attackRange * 64));
            points[1] = new Vector2(64 + (attackRange * 128), 0);
            points[2] = new Vector2(0, 32 + (attackRange * 64));
            points[3] = new Vector2(-64 - (attackRange * 128), 0);
            return points;
        }

        private void Shoot()
        {
            _canShoot = false;
            GetNode<Timer>("ReloadTimer").Start();
            Vector2 pos = GetNode<Position2D>("Node2D/ProjectileSpawn").GlobalPosition;
            PhysicsBody2D target = _targetList[0];
            
            //Emits the signal "ShootEvent" with the following passed variables
            EmitSignal(nameof(ShootEvent), _projectile, pos, target, _towerData.Damage);
        }

        public void OnAttackRangeBodyEntered(PhysicsBody2D body)
        {
            _targetList.Add(body);
        }

        public void OnAttackRangeBodyExited(PhysicsBody2D body)
        {
            if(!_targetList.Contains(body)) return;

            _targetList.Remove(body);
        }

        public void OnReloadTimerTimeout()
        {
            _canShoot = true;
        }
    }
}
