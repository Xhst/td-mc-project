using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Singletons;
using TowerDefenseMC.Utils;


namespace TowerDefenseMC.UserInterface.Menu
{
    public class LevelsMenu : Control
    {
        [Export] 
        private PackedScene _levelButtonScene;

        private Control _buttonsContainer;
        private Game _game;
        
        
        public override void _Ready()
        {
            _buttonsContainer = GetNode<Control>("ButtonsContainer");
            _game = GetNode<Game>("/root/Game");
            
            CreateLevelButtons();
        }

        private void CreateLevelButtons()
        {
            HashSet<string> levels = GetLevels();

            foreach (string level in levels)
            {
                int levelNumber = int.Parse(level);
                
                LevelButton btn = (LevelButton) _levelButtonScene.Instance();
                
                if (_game.TryGetCompletedLevel(levelNumber, out CompletedLevel completedLevel))
                {
                    btn.Init(this, levelNumber, completedLevel.Stars);
                }
                else
                {
                    btn.Init(this, levelNumber, 0, _game.NextLevel != levelNumber);
                }

                _buttonsContainer.AddChild(btn);
            }
        }

        private HashSet<string> GetLevels()
        {
            HashSet<string> levels = new HashSet<string>();
            
            HashSet<string> files = FileHelper.FilesInDirectory("res://assets/data/levels/");

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
