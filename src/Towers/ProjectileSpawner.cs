using Godot;

namespace TowerDefenseMC.Towers
{
    public class ProjectileSpawner
    {
        private readonly Levels.LevelTemplate _levelTemplate;

        public ProjectileSpawner(Levels.LevelTemplate levelTemplate)
        {
            _levelTemplate = levelTemplate;
        }

        public void SpawnProjectile(PackedScene _projectile, Vector2 _pos, PhysicsBody2D _target)
        {
            Node projectile = _projectile.Instance(); //Create a new instance of the projectile to put in the main scene
            ((ProjectileTemplate) projectile).Position = _pos;
            ((ProjectileTemplate) projectile).Rotation = new Vector2(1,0).AngleTo((_target.Position - _pos).Normalized()); //Rotate the projectile depending on where it needs to be shoot
            ((ProjectileTemplate) projectile).Start(_pos, _target);
            _levelTemplate.GetNode<YSort>("ProjectileContainer").AddChild(projectile); //Adds the projectile to the scene
        }
    }
}