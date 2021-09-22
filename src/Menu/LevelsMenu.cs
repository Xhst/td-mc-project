using Godot;

namespace TowerDefenseMC.Menu
{
    public class LevelsMenu : Control
    {
        public void OnBackButtonPressed()
        {
            GetTree().ChangeScene("res://scenes/menu/Menu.tscn");
        }

        public void OnLevelOneButtonPressed()
        {
            GetTree().ChangeScene("res://scenes/levels/LevelTemplate.tscn");
        }
    }
}
