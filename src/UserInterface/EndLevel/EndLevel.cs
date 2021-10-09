using Godot;
using System;

namespace TowerDefenseMC.UserInterface.EndLevel
{
    public class EndLevel : Control
    {
        private Star _star1;
        private Star _star2;
        private Star _star3;

        public override void _Ready()
        {
            _star1 = GetNode<Star>("LevelCompletedScreen/Star1");
            _star2 = GetNode<Star>("LevelCompletedScreen/Star2");
            _star3 = GetNode<Star>("LevelCompletedScreen/Star3");
        }
    }
}
