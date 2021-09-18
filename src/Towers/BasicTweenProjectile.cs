using Godot;

namespace TowerDefenseMC.Towers
{
    public class BasicTweenProjectile : ProjectileTemplate
    {
        public override void Start(Vector2 _pos, PhysicsBody2D _target)
        {
            base.Start(_pos, _target);
            Levels.EnemyTemplate target = (Levels.EnemyTemplate)_target;
            float time = (_pos.DistanceTo(target.GlobalPosition + target.TargetOffset)) / MaxSpeed;
            GetNode<Tween>("Tween").InterpolateProperty(this, //Object
                                                        "position", //Variable that I am changing
                                                        _pos, //Starting position
                                                        target.GlobalPosition + target.TargetOffset, //Ending position
                                                        time, //How long the Tween is going to take
                                                        Tween.TransitionType.Linear, 
                                                        Tween.EaseType.In); 
            GetNode<Tween>("Tween").Start();
        }
    }
}