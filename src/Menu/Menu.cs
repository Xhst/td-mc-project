using Godot;

namespace TowerDefenseMC.Menu
{
    public class Menu : Control
    {
        public void OnLevelsButtonPressed()
        {
            GetNode<Control>("MainMenu").Hide();
            GetNode<Control>("LevelsMenu").Show();
        }

        public void OnSettingsButtonPressed()
        {
            GetNode<Control>("MainMenu").Hide();
            GetNode<Control>("SettingsMenu").Show();
        }

        public void OnBackButtonPressed()
        {
            GetNode<Control>("LevelsMenu").Hide();
            GetNode<Control>("SettingsMenu").Hide();
            GetNode<Control>("MainMenu").Show();
        }

        public void OnGenericMainMenuButtonPressed()
        {
            GetNode<AudioStreamPlayer>("UISounds/ButtonClick").Play();
        }
    }
}
