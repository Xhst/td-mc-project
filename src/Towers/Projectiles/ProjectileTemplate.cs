using Godot;

using TowerDefenseMC.Levels;


namespace TowerDefenseMC.Towers.Projectiles
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

            Vector2 targetPosition = (target.GlobalPosition - pos + target.TargetOffset).Normalized();
            
            Rotation = new Vector2(1,0).AngleTo(targetPosition);
            float rotOffset = Mathf.Abs(new Vector2(1,0).Dot(targetPosition));

            Scale = new Vector2(0.5f + 0.5f * rotOffset, Scale.y);
            
            _speed = (MaxSpeed/2) + ((MaxSpeed/2) * rotOffset);
        }

        private void Movement(float delta)
        {
            Vector2 velocity = new Vector2(_speed * delta, 0);
            
            //Projectile goes to the direction it's pointing
            Position += velocity.Rotated(Rotation); 
        }

        public void OnTimerTimeout()
        {
            QueueFree();
        }
    }
}
