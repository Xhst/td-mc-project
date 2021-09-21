using Godot;

using TowerDefenseMC.Levels;


namespace TowerDefenseMC.Towers.Projectiles
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

            //Adds the projectile to the scene
            _levelTemplate.GetNode<YSort>("ProjectileContainer").AddChild(projectile);

            projectile.Start(pos, target);
        }
    }
}