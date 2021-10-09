using System.Collections;
using System.Collections.Generic;

using Godot;


namespace TowerDefenseMC.Singletons
{

    public readonly struct CompletedLevel
    {
        public readonly int Level;
        public readonly int Stars;
        
        public CompletedLevel(int level = 0, int stars = 0)
        {
            Level = level;
            Stars = stars;
        }
    }
    
    public class Game : Node
    {
        private Dictionary<int, CompletedLevel> _completedLevels;

        public override void _Ready()
        {
            _completedLevels = new Dictionary<int, CompletedLevel>();
        }

        public void LevelCompleted(int levelNumber, int stars)
        {
            CompletedLevel completedLevel = new CompletedLevel(levelNumber, stars);
            
            if (_completedLevels.TryGetValue(levelNumber, out CompletedLevel currentCompletedLevel))
            {
                if (stars <= currentCompletedLevel.Stars) return;

                _completedLevels[levelNumber] = completedLevel;
            }
            else
            {
                _completedLevels.Add(levelNumber, completedLevel);
            }

            GetNode<Persist>("/root/Persist").Save();
        }

        public bool TryGetCompletedLevel(int levelNumber, out CompletedLevel completedLevel)
        {
            return _completedLevels.TryGetValue(levelNumber, out completedLevel);
        }
        
        public Godot.Collections.Dictionary<int,object> GetCompletedLevelsToGodotDictionary()
        {
            Godot.Collections.Dictionary<int,object> dict = new Godot.Collections.Dictionary<int,object>();

            foreach (CompletedLevel completedLevel in _completedLevels.Values)
            {
                dict.Add(completedLevel.Level, completedLevel.Stars);
            }
            
            return dict;
        }

        public void LoadLevelsCompleted(Godot.Collections.Dictionary dict)
        {
            foreach (DictionaryEntry entry in dict)
            {
                _completedLevels.Add(
                    int.Parse(entry.Key.ToString()), 
                    new CompletedLevel(int.Parse(entry.Key.ToString()), int.Parse(entry.Value.ToString()))
                );
            }
        }
    }
}