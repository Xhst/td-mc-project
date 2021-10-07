using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Singletons;

namespace TowerDefenseMC.UserInterface.TopBar
{
    public class TopBar : Control
    {
        public Crystals Crystals;
        public HealthBar HealthBar;
        public WaveTimer WaveTimer;

        [Signal]
        private delegate void PauseMenuButtonPressed();

        public override void _Ready()
        {
            Crystals = GetNode<Crystals>("ColorRect/HBoxContainer/Crystal");
            HealthBar = GetNode<HealthBar>("ColorRect/HBoxContainer/HealthBar");
            WaveTimer = GetNode<WaveTimer>("ColorRect/HBoxContainer/TimerTextContainer/Timer");

            Connect(nameof(PauseMenuButtonPressed), Scenes.MainScene.GetActiveScene(), nameof(LevelTemplate.OnPauseMenuButtonPressed));
        }
        
        public void OnMenuButtonPressed()
        {
            EmitSignal(nameof(PauseMenuButtonPressed));
            WaveTimer = GetNode<WaveTimer>("ColorRect/HBoxContainer/TimerTextContainer/Timer");
        }
    }
}
