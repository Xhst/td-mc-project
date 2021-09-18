using Godot;


namespace TowerDefenseMC.Levels
{
    public class EnemyTemplate : KinematicBody2D
    {
        public PathFollow2D PositioningNode;

        private const float Speed = 150;
        public Vector2 TargetOffset = new Vector2(0, -50);
        
        public override void _PhysicsProcess(float delta)
        {
            if (PositioningNode != null)
            {
                PositioningNode.Offset += Speed * delta;
            }
        }
    }
}