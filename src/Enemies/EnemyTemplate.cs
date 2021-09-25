using Godot;


namespace TowerDefenseMC.Enemies
{
    public class EnemyTemplate : KinematicBody2D
    {
        public PathFollow2D PositioningNode;
        public Vector2 TargetOffset = new Vector2(0, -50);
        
        private float _speed = 150;
        private float _healthPoints = 3;

        private TextureProgress _healthBar;

        public override void _Ready()
        {
            _healthBar = GetNode<TextureProgress>("HealthBar");

            _healthBar.MaxValue = _healthPoints;
            _healthBar.Value = _healthPoints;
        }

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
            _healthBar.Value = _healthPoints;
            
            if (_healthPoints <= 0)
            {
                OnDestroy();
            }
        }

        private void OnDestroy()
        {
            QueueFree();
        }
    }
}