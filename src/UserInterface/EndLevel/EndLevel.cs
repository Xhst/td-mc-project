using Godot;
using System;

using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.UserInterface.EndLevel
{
    public class EndLevel : Control
    {
        private CompletedLevel _completedLevel;
        private TextureProgress _stars;
        private Game _game;

        public override void _Ready()
        {
            _stars = GetNode<TextureProgress>("LevelCompletedScreen/Stars");
            _game = GetNode<Game>("/root/Game");
        }

        public void SetCompletedLevelData(CompletedLevel completedLevel)
        {
            _completedLevel = completedLevel;
            _stars.Value = (float) completedLevel.Stars / 6 * 100;
        }

        public void OnNextLevelButtonPressed()
        {
            Scenes scenes = GetNode<Scenes>("/root/Scenes");
            
            if (_game.LevelsExists(_completedLevel.Level + 1))
            {
                scenes.ChangeScene("res://scenes/levels/LevelTemplate.tscn", _completedLevel.Level + 1);
                return;
            }
            
            scenes.ChangeScene("res://scenes/Main.tscn");
        }

        public void OnShareButtonPressed()
        {

        }

        public void OnBackToMenuButtonPressed()
        {
            GetTree().ChangeScene("res://scenes/Main.tscn");
        }
    }
}
