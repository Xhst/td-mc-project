using Godot;

namespace TowerDefenseMC.Towers
{
    [Tool]
    public class TowerTemplate : Node2D
    {
        private int _attackRange = 1;
        [Export] private int attackRange {
            get {return _attackRange;}
            set {_attackRange = value; SetAttackRange(_attackRange);}
        }

        public override void _Ready()
        {
            
        }

        public override void _PhysicsProcess(float delta)
        {
            
        }

        private void SetAttackRange(int num)
        {
            _attackRange = num;
            ConvexPolygonShape2D _newShape = new ConvexPolygonShape2D();
            _newShape.Points = GetAttackRangeShape(num);
            if(GetNode<CollisionShape2D>("AttackRange/AttackRangeCollision") != null) GetNode<CollisionShape2D>("AttackRange/AttackRangeCollision").Shape = _newShape;
        }

        public Vector2[] GetAttackRangeShape(int _attackRange)
        {
            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(0, -32 - (_attackRange * 64));
            points[1] = new Vector2(64 + (_attackRange * 128), 0);
            points[2] = new Vector2(0, 32 + (_attackRange * 64));
            points[3] = new Vector2(-64 - (_attackRange * 128), 0);
            return points;
        }
    }
}
