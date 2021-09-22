using Godot;

using TowerDefenseMC.Enemies;
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

        public void SpawnProjectile(PackedScene projectileScene, Vector2 pos, PhysicsBody2D targetBody,
            int damage, float projectileSpeed)
        {
            if (!(targetBody is EnemyTemplate target) || projectileSpeed <= 0) return;
            
            ProjectileTemplate projectile = (ProjectileTemplate) projectileScene.Instance();

            //Adds the projectile to the scene
            _levelTemplate.GetNode<YSort>("EntitiesContainer").AddChild(projectile);

            projectile.Start(pos, target, damage, projectileSpeed);
        }
    }
}