using Godot;

namespace TowerDefenseMC.Towers
{
    public class ProjectileTemplate : Area2D
    {
        [Export]
        public float MaxSpeed = 250;
        private float _speed = 250;

        public override void _PhysicsProcess(float delta)
        {
            Movement(delta);
        }

        public virtual void Start(Vector2 _pos, PhysicsBody2D _target)
        {
            Position = _pos;
            Levels.EnemyTemplate target = (Levels.EnemyTemplate)_target;
            Rotation = new Vector2(1,0).AngleTo((target.GlobalPosition - _pos + target.TargetOffset).Normalized());
            float _rotOffset = Mathf.Abs(new Vector2(1,0).Dot((target.GlobalPosition - _pos + target.TargetOffset).Normalized()));
            //Scale.y = 0.5f + (0.5f * _rotOffset);
            _speed = (MaxSpeed/2) + ((MaxSpeed/2)*_rotOffset);
        }

        private void Movement(float delta)
        {
            Vector2 velocity = new Vector2(_speed*delta, 0);
            Position += velocity.Rotated(Rotation); //Projectile goes to the direction it's pointing
        }

        public void OnTweenTweenAllCompleted()
        {
            QueueFree();
        }

        public void OnTimerTimeout()
        {
            QueueFree();
        }
    }
}
