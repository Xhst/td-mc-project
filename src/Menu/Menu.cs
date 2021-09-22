using Godot;

namespace TowerDefenseMC.Menu
{
    public class Menu : Control
    {
        public void OnLevelButtonPressed()
        {
            GetTree().ChangeScene("res://scenes/menu/LevelsMenu.tscn");
        }
    }
}
