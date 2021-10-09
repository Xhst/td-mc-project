using Godot;


namespace TowerDefenseMC.UserInterface.Menu
{
    public class LevelButton : Control
    {
        private LevelsMenu _menu;
        
        private int _targetLevel;

        private Button _button;
        private TextureProgress _stars;

        public void Init(LevelsMenu menu, int level, int stars, bool disabled = false)
        {
            _menu = menu;
            _targetLevel = level;

            _button = GetNode<Button>("LevelButton");
            _button.Text = level.ToString();
            _button.Disabled = disabled;
            
            _stars = GetNode<TextureProgress>("LevelButton/Stars");
            _stars.Value = (float) stars / 6 * 100;
        }
        
        public void OnLevelButtonPressed()
        {
            _menu.OnLevelButtonPressed(_targetLevel);
        }
    }
}