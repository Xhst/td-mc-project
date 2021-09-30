using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.UserInterface.Menu
{
    public class SettingsMenu : Control
    {
        private List<string> _languageList;

        private TextureButton _soundButton;
        private TextureButton _musicButton;

        private bool _changeAudioSetting = false;

        public override void _Ready()
        {
            _languageList = LanguageManager.GetAvailableLanguages();

            _soundButton = GetNode<TextureButton>("SoundSetting/SoundOnOff");
            _musicButton = GetNode<TextureButton>("MusicSetting/MusicOnOff");
        }

        public override void _PhysicsProcess(float delta)
        {
            if(_changeAudioSetting)
            {
                if(_soundButton.Pressed)
                {
                    AudioServer.SetBusMute(AudioServer.GetBusIndex("Sound"), false);
                }
                else
                {
                    AudioServer.SetBusMute(AudioServer.GetBusIndex("Sound"), true);
                }

                if(_musicButton.Pressed)
                {
                    AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), false);
                }
                else
                {
                    AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), true);
                }

                _changeAudioSetting = false;
            }
        }

        public void OnLeftButtonPressed()
        {
            Label lenguage = GetNode<Label>("LenguageSetting/LenguageText");
            int index = _languageList.IndexOf(lenguage.Text);
            
            lenguage.Text = index - 1 < 0 ? _languageList[_languageList.Count - 1] : _languageList[index - 1];
        }

        public void OnRightButtonPressed()
        {
            Label lenguage = GetNode<Label>("LenguageSetting/LenguageText");
            int index = _languageList.IndexOf(lenguage.Text);
            
            lenguage.Text = index + 1 == _languageList.Count ? _languageList[0] : _languageList[index + 1];
        }

        public void OnAudioButtonPressed()
        {
           _changeAudioSetting = true;
        }
    }
}
