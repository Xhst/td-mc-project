using Godot;

using TowerDefenseMC.Enemies;
using TowerDefenseMC.Levels;


namespace TowerDefenseMC.Towers.Projectiles
{
    public class ProjectileTemplate : Area2D
    {
        protected float MaxSpeed = 250;
        protected float Speed = 250;
        protected int Damage = 1;

        public override void _PhysicsProcess(float delta)
        {
            Movement(delta);
        }

        public virtual void Start(Vector2 pos, EnemyTemplate target, int damage)
        {
            Position = pos;
            Damage = damage;

            Vector2 targetPosition = (target.GlobalPosition - pos + target.TargetOffset).Normalized();
            
            Rotation = new Vector2(1,0).AngleTo(targetPosition);
            float rotOffset = Mathf.Abs(new Vector2(1,0).Dot(targetPosition));

            Scale = new Vector2(0.5f + 0.5f * rotOffset, Scale.y);
            
            Speed = (MaxSpeed/2) + ((MaxSpeed/2) * rotOffset);

            GetNode<Timer>("Timer").Start();
        }

        private void Movement(float delta)
        {
            Vector2 velocity = new Vector2(Speed * delta, 0);
            
            //Projectile goes to the direction it's pointing
            Position += velocity.Rotated(Rotation); 
        }

        public void OnProjectileBodyEntered(PhysicsBody2D body)
        {
            if (!(body is EnemyTemplate target)) return;
            
            target.TakeDamage(Damage);
            
            QueueFree();
        }

        public void OnTimerTimeOut()
        {
            QueueFree();
        }
    }
}
