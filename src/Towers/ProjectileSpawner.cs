using Godot;

using TowerDefenseMC.Levels;


namespace TowerDefenseMC.Towers
{
    public class ProjectileSpawner
    {
        private readonly LevelTemplate _levelTemplate;

        public ProjectileSpawner(LevelTemplate levelTemplate)
        {
            _levelTemplate = levelTemplate;
        }

        public void SpawnProjectile(PackedScene projectileScene, Vector2 pos, PhysicsBody2D targetBody)
        {
            if (!(targetBody is EnemyTemplate target)) return;
            
            ProjectileTemplate projectile = (ProjectileTemplate) projectileScene.Instance(); 
            projectile.Position = pos;
        
            //Rotate the projectile depending on where it needs to be shoot
            projectile.Rotation = new Vector2(1,0).AngleTo((target.Position - pos).Normalized());
            projectile.Start(pos, target);
        
            //Adds the projectile to the scene
            _levelTemplate.GetNode<YSort>("ProjectileContainer").AddChild(projectile);
            
        }
    }
}