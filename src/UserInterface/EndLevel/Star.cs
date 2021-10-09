using Godot;
using System;

namespace TowerDefenseMC.UserInterface.EndLevel
{
    public class Star : Control
    {
        private NinePatchRect _filledStar;

        public override void _Ready()
        {
            _filledStar = GetNode<NinePatchRect>("FilledStar");
        }

        public void SetFilledStarVisibilty(bool isVisible)
        {
            _filledStar.Visible = isVisible;
        }
    } 
}
