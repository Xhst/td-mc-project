using Godot;


namespace TowerDefenseMC.UserInterface.Menu
{
    public class LevelsMenu : Control
    {
        public void OnLevelButtonPressed()
        {
            GetNode<AudioStreamPlayer>("LevelButtonClickSound").Play();
        }
    }
}
