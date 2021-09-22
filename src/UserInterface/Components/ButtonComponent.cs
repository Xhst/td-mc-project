using Godot;

using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.UserInterface.Components
{
    public class ButtonComponent : Button
    {
        public override void _Ready()
        {
            Text = LanguageManager.UI(Text);
        }
    }
}