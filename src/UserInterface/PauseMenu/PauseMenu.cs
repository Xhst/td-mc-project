using Godot;
using System;

using TowerDefenseMC.Singletons;

namespace TowerDefenseMC.UserInterface.PauseMenu
{
    public class PauseMenu : Control
    {
        private bool _hasAudioSettingsChanged;

        private TextureButton _soundButton;
        private TextureButton _musicButton;

        private AudioStreamPlayer _clickSound;

        public override void _Ready()
        {
            _soundButton = GetNode<TextureButton>("SoundSetting/SoundOnOff");
            _musicButton = GetNode<TextureButton>("MusicSetting/MusicOnOff");

            _clickSound = GetNode<AudioStreamPlayer>("ButtonClickSound");

            SetSoundButtonPressed(!Audio.SoundPressed);
            SetMusicButtonPressed(!Audio.MusicPressed);
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_hasAudioSettingsChanged)
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Sound"), !_soundButton.Pressed);
                Audio.SoundPressed = !_soundButton.Pressed;

                AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), !_musicButton.Pressed);
                Audio.MusicPressed = !_musicButton.Pressed;

                _hasAudioSettingsChanged = false;
            }
        }

        public void SetSoundButtonPressed(bool pressed)
        {
            _soundButton.Pressed = pressed;
        }

        public void SetMusicButtonPressed(bool pressed)
        {
            _musicButton.Pressed = pressed;
        }

        public void SetPauseMode(bool pauseMode)
        {
            GetTree().Paused = pauseMode;
            Visible = pauseMode;
        }

        public void OnContinueButtonPressed()
        {
            _clickSound.Play();

            SetPauseMode(false);
        }

        public void OnBackToMenuButtonPressed()
        {
            _clickSound.Play();

            GetTree().ChangeScene("res://scenes/Main.tscn");
            GetTree().Paused = false;
        }

        public void OnAudioSettingPressed()
        {
            _hasAudioSettingsChanged = true;
        }
    }
}