using Godot;

using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.UserInterface.Statistics
{
    [Tool]
    public class StatisticName : Label
    {
        private string _text;
        [Export]
        public new string Text
        {
            get => _text;
            set
            {
                _text = value;
                base.Text = LanguageManager.UI(value);
            }
        }

        public override void _Ready()
        {
            base.Text = LanguageManager.UI(base.Text);
        }
    }
}
