using System;

using Godot;


namespace TowerDefenseMC.UserInterface.TopBar
{
    public class WaveTimer : Label
    {
        private Timer _waveTimer;

        public void SetWaveTimer(Timer waveTimer)
        {
            _waveTimer = waveTimer;
        }

        public override void _Process(float delta)
        {
            if (_waveTimer == null) return;
            
            float timeLeft = _waveTimer.TimeLeft;
            Text = $"{(int) Math.Floor(timeLeft / 60):D2}:{(int) timeLeft % 60:D2}";
        }
    }
}