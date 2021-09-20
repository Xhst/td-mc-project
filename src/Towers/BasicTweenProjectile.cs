using Godot;

using TowerDefenseMC.Levels;


namespace TowerDefenseMC.Towers
{
    public class BasicTweenProjectile : ProjectileTemplate
    {
        public override void Start(Vector2 pos, EnemyTemplate target)
        {
            base.Start(pos, target);

            float time = (pos.DistanceTo(target.GlobalPosition + target.TargetOffset)) / MaxSpeed;
            GetNode<Tween>("Tween").InterpolateProperty(this, //Object
                                                        "position", //Variable that I am changing
                                                        pos, //Starting position
                                                        target.GlobalPosition + target.TargetOffset, //Ending position
                                                        time, //How long the Tween is going to take
                                                        Tween.TransitionType.Linear, 
                                                        Tween.EaseType.In); 
            GetNode<Tween>("Tween").Start();
        }
    }
}