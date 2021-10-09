using Godot;


namespace TowerDefenseMC.UserInterface.Menu
{
    public class LevelButton : Control
    {
        private LevelsMenu _menu;
        
        private int _targetLevel;
        private int _stars;

        private Button _button;

        public void Init(LevelsMenu menu, int level, int stars, bool disabled = false)
        {
            _menu = menu;
            _targetLevel = level;
            _stars = stars;

            _button = GetNode<Button>("LevelButton");
            _button.Text = level.ToString();
            _button.Disabled = disabled;
        }

        public Button GetButton()
        {
            return _button;
        }
        public void OnLevelButtonPressed()
        {
            _menu.OnLevelButtonPressed(_targetLevel);
        }
    }
}