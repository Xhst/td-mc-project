using Godot;

using TowerDefenseMC.Levels;


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

        public virtual void Start(Vector2 pos, EnemyTemplate target)
        {
            Position = pos;
            
            Rotation = new Vector2(1,0).AngleTo((target.GlobalPosition - pos + target.TargetOffset).Normalized());
            float rotOffset = Mathf.Abs(new Vector2(1,0).Dot((target.GlobalPosition - pos + target.TargetOffset).Normalized()));

            Scale = new Vector2(0.5f + 0.5f * rotOffset, Scale.y);
            
            _speed = (MaxSpeed/2) + ((MaxSpeed/2) * rotOffset);
        }

        private void Movement(float delta)
        {
            Vector2 velocity = new Vector2(_speed * delta, 0);
            
            //Projectile goes to the direction it's pointing
            Position += velocity.Rotated(Rotation); 
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
