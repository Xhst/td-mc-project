using System.Collections.Generic;

using Godot;

namespace TowerDefenseMC.Menu
{
    public class SettingsMenu : Control
    {
        private List<string> _lenguageList = new List<string>() {
            " English",
            " Italiano"
        };

        public void OnLeftButtonPressed()
        {
            Label lenguage = GetNode<Label>("LenguageSetting/LenguageText");
            int index = _lenguageList.IndexOf(lenguage.Text);
            if((index - 1) < 0)
            {
                lenguage.Text = _lenguageList[_lenguageList.Count - 1];
            } else {
                lenguage.Text = _lenguageList[index - 1];
            }
        }

        public void OnRightButtonPressed()
        {
            Label lenguage = GetNode<Label>("LenguageSetting/LenguageText");
            int index = _lenguageList.IndexOf(lenguage.Text);
            if((index + 1) == _lenguageList.Count)
            {
                lenguage.Text = _lenguageList[0];
            } else {
                lenguage.Text = _lenguageList[index + 1];
            }
        }
    }
}
