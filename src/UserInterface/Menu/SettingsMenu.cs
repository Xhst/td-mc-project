using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.UserInterface.Menu
{
    public class SettingsMenu : Control
    {
        private List<string> _languageList;

        public override void _Ready()
        {
            _languageList = LanguageManager.GetAvailableLanguages();
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
    }
}
