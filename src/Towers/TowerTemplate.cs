using System.Collections.Generic;

using Godot;

namespace TowerDefenseMC.Towers
{
    [Tool]
    public class TowerTemplate : Node2D
    {
        private bool _canShoot = true;

        private int _attackRange = 1;

        [Signal]
        public delegate void shoot();

        [Export] 
        private int AttackRange 
        {
            get => _attackRange;
            set {_attackRange = value; SetAttackRange(_attackRange);}
        }

        [Export]
        private PackedScene Projectile;
        private List<PhysicsBody2D> _targetList;

        public override void _Ready()
        {
            _targetList = new List<PhysicsBody2D>();
            
            Connect("shoot", Singletons.Globals._mainGameNode, "SpawnProjectile"); //Connects the signal "shoot" with the function "SpawnProjectile" passed by the Object "_mainGameNode"
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
            _attackRange = num;
            ConvexPolygonShape2D newShape = new ConvexPolygonShape2D { Points = GetAttackRangeShape(num) };
            
            if(GetNode<CollisionShape2D>("AttackRange/AttackRangeCollision") != null) GetNode<CollisionShape2D>("AttackRange/AttackRangeCollision").Shape = newShape;
        }

        public Vector2[] GetAttackRangeShape(int attackRange)
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
            EmitSignal("shoot", Projectile, pos, target); //Emits the signal "shoot" with the following passed variables
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
