using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.UserInterface.Menu
{
    public class SettingsMenu : Control
    {
        private LanguageManager _languageManager;
        private List<string> _languageList;

        private TextureButton _soundButton;
        private TextureButton _musicButton;
        private Label _languageTextLabel;

        private bool _hasAudioSettingsChanged = false;

        public override void _Ready()
        {
            _languageManager = GetNode<LanguageManager>("/root/LanguageManager");
            _languageList = new List<string>(_languageManager.GetAvailableLanguages());
            _languageTextLabel = GetNode<Label>("LanguageSetting/LanguageText");
            
            _soundButton = GetNode<TextureButton>("SoundSetting/SoundOnOff");
            _musicButton = GetNode<TextureButton>("MusicSetting/MusicOnOff");
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_hasAudioSettingsChanged)
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Sound"), !_soundButton.Pressed);

                AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), !_musicButton.Pressed);

                _hasAudioSettingsChanged = false;
            }
        }

        public void OnLeftButtonPressed()
        {
            int index = _languageList.IndexOf(_languageTextLabel.Text);
            
            _languageTextLabel.Text = index - 1 < 0 ? _languageList[_languageList.Count - 1] : _languageList[index - 1];
            _languageManager.SetLanguage(_languageTextLabel.Text);
        }

        public void OnRightButtonPressed()
        {
            int index = _languageList.IndexOf(_languageTextLabel.Text);
            
            _languageTextLabel.Text = index + 1 == _languageList.Count ? _languageList[0] : _languageList[index + 1];
            _languageManager.SetLanguage(_languageTextLabel.Text);
        }

        public void OnAudioButtonPressed()
        {
           _hasAudioSettingsChanged = true;
        }
    }
}
