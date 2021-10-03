using System;

using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.Enemies
{
    public class EnemyTemplate : KinematicBody2D
    {
        public PathFollow2D PositioningNode;
        public Vector2 TargetOffset = new Vector2(0, -50);
        
        private float _speed = 150;
        private float _healthPoints = 3;
        private float _damage = 5;

        private TextureProgress _healthBar;

        [Signal]
        private delegate void EndOfPathReached(float damage);

        public override void _Ready()
        {
            _healthBar = GetNode<TextureProgress>("HealthBar");

            _healthBar.MaxValue = _healthPoints;
            _healthBar.Value = _healthPoints;

            Connect(nameof(EndOfPathReached), Scenes.MainScene.GetActiveScene(), nameof(LevelTemplate.OnEnemyReachEndOfPath));
        }

        public override void _PhysicsProcess(float delta)
        {
            Move(delta);
            CheckEndOfPath();
        }

        private void Move(float delta)
        {
            if (PositioningNode == null) return;

            PositioningNode.Offset += _speed * delta;
        }

        private void CheckEndOfPath()
        {
            if (Math.Abs(PositioningNode.UnitOffset - 1f) > 0) return;
            
            EmitSignal(nameof(EndOfPathReached), _damage);
            QueueFree();
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
            Game.EnemyIsDead = true;
            QueueFree();
        }
    }
}