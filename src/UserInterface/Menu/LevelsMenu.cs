using Godot;


namespace TowerDefenseMC.UserInterface.Menu
{
    public class LevelsMenu : Control
    {
        public void OnLevelOneButtonPressed()
        {
            GetTree().ChangeScene("res://scenes/levels/LevelTemplate.tscn");
        }

        public void OnGenericLevelButtonPressed()
        {
            GetNode<AudioStreamPlayer>("UISounds/LevelButtonClick").Play();
        }
    }
}
