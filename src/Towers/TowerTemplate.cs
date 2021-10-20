using System.Collections.Generic;
using System.Threading.Tasks;

using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.Towers
{

    [Tool]
    public class TowerTemplate : Node2D
    {
        private static float MaxAttackSpeed = 4f;
        
        private bool _canShoot = true;

        private Vector2 _position;
        private TowerData _towerData;
        private LevelTemplate _level;
        private Timer _reloadTimer;
        private Vector2 _projectileSpawnPosition;
        
        private Dictionary<string, TowerEffect> _effects;

        private float _damage;
        private float _attackSpeed;
        

        [Signal]
        private delegate void ShootEvent(int damage, float projectileSpeed);

        [Signal]
        private delegate void TouchEvent(string bind);

        [Export]
        private PackedScene _projectile;
        
        private List<PhysicsBody2D> _targetList;

        public override void _Ready()
        {
            _effects = new Dictionary<string, TowerEffect>();
            _targetList = new List<PhysicsBody2D>();
            _reloadTimer = GetNode<Timer>("ReloadTimer");
            _projectileSpawnPosition = GetNode<Position2D>("Node2D/ProjectileSpawn").GlobalPosition;

            Connect(nameof(ShootEvent), Scenes.MainScene.GetActiveScene(), nameof(LevelTemplate.SpawnProjectile)); 
            Connect(nameof(TouchEvent), Scenes.MainScene.GetActiveScene(), nameof(LevelTemplate.OnTouchScreenButtonReleased));
        }

        public override void _PhysicsProcess(float delta)
        {
            if(_targetList.Count != 0 && _canShoot)
            {
                Shoot();
            }
        }
        
        public void Init(TowerData towerData, LevelTemplate level, Vector2 position)
        {
            _towerData = towerData;
            _level = level;
            _position = position;
            _damage = towerData.Damage;
            _attackSpeed = towerData.AttackSpeed;

            if (_towerData.Damage > 0 && _towerData.AttackSpeed > 0)
            {
                _canShoot = true;
            }
        }

        public static Vector2[] GetRangeShapePoints(int attackRange)
        {
            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(0, -32 - (attackRange * 64));
            points[1] = new Vector2(64 + (attackRange * 128), 0);
            points[2] = new Vector2(0, 32 + (attackRange * 64));
            points[3] = new Vector2(-64 - (attackRange * 128), 0);
            return points;
        }

        private void Shoot()
        {
            _canShoot = false;

            if (_attackSpeed > 0)
            {
                _reloadTimer.WaitTime = 1 / _attackSpeed;
                _reloadTimer.Start();
            }
            
            PhysicsBody2D target = _targetList[0];
            
            //Emits the signal "ShootEvent" with the following passed variables
            EmitSignal(nameof(ShootEvent), _projectile, _projectileSpawnPosition, target, _damage, _towerData.ProjectileSpeed);
        }

        public float GetEffectDamageAdded()
        {
            return _damage - _towerData.Damage;
        }

        public float GetEffectAttackSpeedAdded()
        {
            return _attackSpeed - _towerData.AttackSpeed;
        }

        public async void OnPlace()
        {
            _level.TowerPlaced(_position, this);
            
            await BuildingProcess();
        }

        public void ApplyAura()
        {
            HashSet<TowerTemplate> towersOnAuraRange = _level.GetTowersOnArea(_position, _towerData.AuraRange);

            foreach (TowerTemplate tower in towersOnAuraRange)
            {
                ShaderMaterial shaderMaterial = null;
                
                if (_towerData.AuraShaderMaterialName != "")
                {
                    shaderMaterial = ResourceLoader
                        .Load<ShaderMaterial>($"res://assets/shaders/{ _towerData.AuraShaderMaterialName }.material");
                }

                tower.ApplyEffect(_towerData.AuraEffectName, _towerData.AuraDamage, _towerData.AuraAttackSpeed, shaderMaterial);
            }
        }
        
        private void ApplyEffect(string effectName, float auraDamage, float auraAttackSpeed, ShaderMaterial shaderMaterial = null)
        {
            if (_effects.ContainsKey(effectName)) return;
            
            TowerEffect towerEffect = new TowerEffect(effectName, auraDamage, auraAttackSpeed, shaderMaterial);
            _effects.Add(towerEffect.Name, towerEffect);

            CalculateDamage();
            CalculateAttackSpeed();

            if (shaderMaterial != null) ApplyShader(shaderMaterial);
        }

        private void ApplyShader(ShaderMaterial shaderMaterial)
        {
            Godot.Collections.Array children = GetNode<Node2D>("Node2D").GetChildren();

            foreach (object child in children)
            {
                if (!(child is Sprite sprite)) continue;

                sprite.Material = shaderMaterial;
            }
        }

        private void CalculateDamage()
        {
            float bonusDamage = 0;
            
            foreach (TowerEffect effect in _effects.Values)
            {
                bonusDamage += effect.Damage;
            }

            _damage = _towerData.Damage + _towerData.Damage * bonusDamage / 100;
        }

        private void CalculateAttackSpeed()
        {
            float bonusAttackSpeed = 0;
            
            foreach (TowerEffect effect in _effects.Values)
            {
                bonusAttackSpeed += effect.AttackSpeed;
            }

            _attackSpeed = _towerData.AttackSpeed * (1 + bonusAttackSpeed);

            if (_attackSpeed > MaxAttackSpeed)
            {
                _attackSpeed = MaxAttackSpeed;
            }
        }

        private async Task BuildingProcess()
        {
            Godot.Collections.Array children = GetNode<Node2D>("Node2D").GetChildren();

            foreach (object child in children)
            {
                if (!(child is Sprite sprite)) continue;

                sprite.Visible = false;
            }
            
            foreach (object child in children)
            {
                if (!(child is Sprite sprite)) continue;

                await ToSignal(GetTree().CreateTimer(0.125f), "timeout");
                sprite.Visible = true;
            }
        }

        public Dictionary<string, TowerEffect> GetEffects()
        {
            return _effects;
        }

        public void OnAttackRangeBodyEntered(PhysicsBody2D body)
        {
            _targetList.Add(body);
        }

        public void OnAttackRangeBodyExited(PhysicsBody2D body)
        {
            if(!_targetList.Contains(body)) return;

            _targetList.Remove(body);
        }

        public void OnReloadTimerTimeout()
        {
            _canShoot = true;
        }

        public void OnTouchScreenButtonReleased(string towerName)
        {
            EmitSignal(nameof(TouchEvent), towerName, this);
        }
    }
}
