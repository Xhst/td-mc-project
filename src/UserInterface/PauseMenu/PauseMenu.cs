using Godot;
using System;

namespace TowerDefenseMC.UserInterface.PauseMenu
{
    public class PauseMenu : Control
    {
        public new void PauseMode()
        {
            bool pauseMode = !GetTree().Paused;
            GetTree().Paused = pauseMode;
            Visible = pauseMode;
        }
    }
}
