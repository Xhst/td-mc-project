using Godot;


namespace TowerDefenseMC.Enemies
{
    public class EnemyTemplate : KinematicBody2D
    {
        public PathFollow2D PositioningNode;
        public Vector2 TargetOffset = new Vector2(0, -50);

        private float _speed = 150;
        private int _healthPoints = 3;
        
        
        public override void _PhysicsProcess(float delta)
        {
            if (PositioningNode != null)
            {
                PositioningNode.Offset += _speed * delta;
            }
        }

        public void TakeDamage(int damage)
        {
            _healthPoints -= damage;

            GD.Print(_healthPoints);
            if (_healthPoints < 0)
            {
                Death();
            }
        }

        private void Death()
        {
            QueueFree();
        }
    }
}