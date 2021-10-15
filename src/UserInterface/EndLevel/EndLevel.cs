using Godot;
using System;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.UserInterface.EndLevel
{
    public class EndLevel : Control
    {
        private LevelTemplate _levelTemplate;

        private Scenes _scenes;
        private CompletedLevel _completedLevel;
        private TextureProgress _stars;
        private Game _game;

        private Control _levelCompletedButtons;
        private Control _gameOverButtons;

        public override void _Ready()
        {
            _stars = GetNode<TextureProgress>("Stars");
            _game = GetNode<Game>("/root/Game");
            _scenes = GetNode<Scenes>("/root/Scenes");

            _levelCompletedButtons = GetNode<Control>("LevelCompletedButtons");
            _gameOverButtons = GetNode<Control>("GameOverButtons");
        }

        public void SetCompletedLevelData(CompletedLevel completedLevel)
        {
            _completedLevel = completedLevel;
            _stars.Value = (float) _completedLevel.Stars / 6 * 100;

            _levelCompletedButtons.Show();
            _gameOverButtons.Hide();
        }

        public void SetGameOverScreen()
        {
            _stars.Value = 0;
            GetNode("Title").Set("Tag", "end_level.game_over");

            _gameOverButtons.Show();
            _levelCompletedButtons.Hide();
        }

        public void SetLevelTemplate(LevelTemplate levelTemplate)
        {
            _levelTemplate = levelTemplate;
        }

        public void OnNextLevelButtonPressed()
        {
            if (_game.LevelsExists(_completedLevel.Level + 1))
            {
                _scenes.ChangeScene("res://scenes/levels/LevelTemplate.tscn", _completedLevel.Level + 1);
                return;
            }
            
            _scenes.ChangeScene("res://scenes/Main.tscn");
        }

        public void OnRetryLevelButtonPressed()
        {
            
        }

        public void OnShareButtonPressed()
        {

        }

        public void OnBackToMenuButtonPressed()
        {
            _scenes.ChangeScene("res://scenes/Main.tscn");
        }
    }
}
