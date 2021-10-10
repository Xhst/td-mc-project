using Godot;
using System;

using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.UserInterface.EndLevel
{
    public class EndLevel : Control
    {
        private Scenes _scenes;
        private CompletedLevel _completedLevel;
        private TextureProgress _stars;
        private Game _game;

        public override void _Ready()
        {
            _stars = GetNode<TextureProgress>("LevelCompletedScreen/Stars");
            _game = GetNode<Game>("/root/Game");
            _scenes = GetNode<Scenes>("/root/Scenes");
        }

        public void SetCompletedLevelData(CompletedLevel completedLevel)
        {
            _completedLevel = completedLevel;
            _stars.Value = (float) completedLevel.Stars / 6 * 100;
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

        public void OnShareButtonPressed()
        {

        }

        public void OnBackToMenuButtonPressed()
        {
            _scenes.ChangeScene("res://scenes/Main.tscn");
        }
    }
}
