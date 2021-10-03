using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Utils;


namespace TowerDefenseMC.UserInterface.TopBar
{
    public class HealthBar : TextureProgress, IObserver
    {
        private Player _player;

        private Tween _tween;

        public override void _Ready()
        {
            _tween = GetNode<Tween>("Tween");
            MaxValue = 100;
        }

        public void SetPlayer(Player player)
        {
            _player = player;
            _player.Attach(this);
            MaxValue = _player.MaxHealth;
            Value = _player.Health;
            Update();
        }

        public new void Update()
        {
            _tween.InterpolateProperty(this, "value", Value, _player.Health, 0.1f);
            _tween.Start();
        }
    }
}