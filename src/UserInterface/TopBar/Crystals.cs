using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Utils;


namespace TowerDefenseMC.UserInterface.TopBar
{
    public class Crystals : Label, IObserver
    {
        private Player _player;

        public void SetPlayer(Player player)
        {
            _player = player;
            _player.Attach(this);
            Update();
        }

        public new void Update()
        {
            Text = _player.Crystals.ToString();
        }
    }
}