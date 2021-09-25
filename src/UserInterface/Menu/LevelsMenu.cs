using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Singletons;
using TowerDefenseMC.Utils;


namespace TowerDefenseMC.UserInterface.Menu
{
    public class LevelsMenu : Control
    {
        [Export] 
        private PackedScene _levelButtonScene;
        
        private Control _buttonsContainer;
        
        
        public override void _Ready()
        {
            _buttonsContainer = GetNode<Control>("ButtonsContainer");
            CreateLevelButtons();
        }

        private void CreateLevelButtons()
        {
            HashSet<string> levels = GetLevels();

            foreach (string level in levels)
            {
                LevelButton btn = (LevelButton) _levelButtonScene.Instance();
                btn.Init(this, int.Parse(level));

                _buttonsContainer.AddChild(btn);
            }
        }

        private HashSet<string> GetLevels()
        {
            HashSet<string> levels = new HashSet<string>();
            
            List<string> files = FileHelper.FilesInDirectory("res://assets/data/levels/");

            foreach (string file in files)
            {
                string level = file.Replace(".json", "").Replace("level", "");
                levels.Add(level);
            }

            return levels;
        }

        public void OnLevelButtonPressed(int level)
        {
            Scenes scenes = GetNode<Scenes>("/root/Scenes");
            
            scenes.ChangeScene("res://scenes/levels/LevelTemplate.tscn", level);
        }
    }
}
