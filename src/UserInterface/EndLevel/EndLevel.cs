using Godot;
using System;

namespace TowerDefenseMC.UserInterface.EndLevel
{
    public class EndLevel : Control
    {
        private TextureProgress _stars;

        public override void _Ready()
        {
            _stars = GetNode<TextureProgress>("LevelCompletedScreen/Stars");
        }

        public void SetStars(int stars)
        {
            _stars.Value = (float) stars / 6 * 100;
        }

        public void OnNextLevelButtonPressed()
        {
            
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
