using Godot;


namespace TowerDefenseMC.UserInterface.Menu
{
    public class LevelButton : Control
    {
        private LevelsMenu _menu;
        private int _targetLevel;

        public void Init(LevelsMenu menu, int level)
        {
            _menu = menu;
            _targetLevel = level;
            
            GetNode<Button>("LevelButton").Text = level.ToString();
        }

        public void OnLevelButtonPressed()
        {
            _menu.OnLevelButtonPressed(_targetLevel);
        }
    }
}