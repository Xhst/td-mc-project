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
            HashSet<int> levels = _game.GetAllLevels();

            foreach (int level in levels)
            {
                LevelButton btn = (LevelButton) _levelButtonScene.Instance();
                
                if (_game.TryGetCompletedLevel(level, out CompletedLevel completedLevel))
                {
                    btn.Init(this, level, completedLevel.Stars);
                }
                else
                {
                    btn.Init(this, level, 0, _game.NextLevel != level);
                }

                _buttonsContainer.AddChild(btn);
            }
        }

        public void OnLevelButtonPressed(int level)
        {
            Scenes scenes = GetNode<Scenes>("/root/Scenes");
            
            scenes.ChangeScene("res://scenes/levels/LevelTemplate.tscn", level);
        }
    }
}
