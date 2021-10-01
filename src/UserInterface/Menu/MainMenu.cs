using System.Collections.Generic;

using Godot;

namespace TowerDefenseMC.UserInterface.Menu
{
	public class MainMenu : Control
	{
		private enum Menu
		{
			MainMenu,
			LevelsMenu,
			SettingsMenu
		}

		private Dictionary<Menu, Control> _menus;

		private AudioStreamPlayer _clickSound;
		public override void _Ready()
		{
			_menus = new Dictionary<Menu, Control>
			{
				{ Menu.MainMenu, GetNode<Control>("MainMenu") },
				{ Menu.LevelsMenu, GetNode<Control>("LevelsMenu") },
				{ Menu.SettingsMenu, GetNode<Control>("SettingsMenu") }
			};

			_clickSound = GetNode<AudioStreamPlayer>("ButtonClickSound");
		}

		private void ShowMenu(Menu menu)
		{
			foreach (KeyValuePair<Menu, Control> kv in _menus)
			{
				if (kv.Key == menu)
				{
					kv.Value.Show();
					continue;
				}

				kv.Value.Hide();
			}
		}

		public void OnLevelsButtonPressed()
		{
			ShowMenu(Menu.LevelsMenu);
			_clickSound.Play();
		}
		
		public void OnSettingsButtonPressed()
		{
			ShowMenu(Menu.SettingsMenu);
			_clickSound.Play();
		}

		public void OnShareButtonPressed()
		{
			_clickSound.Play();
		}

		public void OnBackButtonPressed()
		{
			ShowMenu(Menu.MainMenu);
			_clickSound.Play();
		}
	}
}
