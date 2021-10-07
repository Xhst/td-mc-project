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
        private float _health = 3;
        private float _damage = 1;
        private int _feed = 1;

        private Sprite _sprite;
        private TextureProgress _healthBar;

        [Signal]
        private delegate void EndOfPathReached(float damage);

        [Signal]
        private delegate void EnemyDestroyed(int feed);

        public override void _Ready()
        {
            _sprite = GetNode<Sprite>("SpriteContainer/Sprite");
            _healthBar = GetNode<TextureProgress>("HealthBar");

            _healthBar.MaxValue = _health;
            _healthBar.Value = _health;

            Connect(nameof(EndOfPathReached), Scenes.MainScene.GetActiveScene(), nameof(LevelTemplate.OnEnemyReachEndOfPath));
            Connect(nameof(EnemyDestroyed), Scenes.MainScene.GetActiveScene(), nameof(LevelTemplate.OnEnemyDestroyed));
        }

        public void Init(EnemyData enemyData)
        {
            _damage = enemyData.Damage;
            _feed = enemyData.Feed;
            _health = enemyData.Health;
            _speed = enemyData.Speed;
            
            _sprite.Texture = ResourceLoader.Load<Texture>($"res://assets/img/enemies/{ enemyData.Image }.png");
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

        public void TakeDamage(float damage)
        {
            _health -= damage;
            _healthBar.Value = _health;
            
            if (_health <= 0)
            {
                OnDestroy();
            }
        }

        private void OnDestroy()
        {
            EmitSignal(nameof(EnemyDestroyed), _feed);
            QueueFree();
        }
    }
}