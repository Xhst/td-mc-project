using System;
using System.Collections.Generic;

using Godot;

using Newtonsoft.Json.Linq;

using TowerDefenseMC.Utils;


namespace TowerDefenseMC.Levels
{

    public struct LevelData
    {
        public bool isSnowy;
        public Dictionary<string, List<TilePosition>> tiles;
    }
    public struct TilePosition
    {
        public int x;
        public int y;
        public Rotation rot;
    }
    
    public class LevelDataReader : Node
    {
        public LevelData GetLevelData(int level)
        {
            LevelData levelData;
            
            levelData.tiles =  new Dictionary<string, List<TilePosition>>();
            
            string path = ProjectSettings.GlobalizePath($"res://assets/levels/level{level}.json");
            string jsonFileText = System.IO.File.ReadAllText(path);
            
            JObject json = JObject.Parse(jsonFileText);

            bool isSnowy = json["snowy"]?.ToString() == bool.TrueString;
            levelData.isSnowy = isSnowy;
            
            if (json["tiles"] == null) return levelData;

            levelData.tiles = GetTilesData(isSnowy, json["tiles"]);
            

            return levelData;
        }

        private Dictionary<string, List<TilePosition>> GetTilesData(bool isSnowy, JToken jsonTiles)
        {
            Dictionary<string, List<TilePosition>> tileData = new Dictionary<string, List<TilePosition>>();
            
            JArray tiles = JArray.Parse(jsonTiles.ToString());

            foreach (JObject content in tiles.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string tileName = isSnowy ? "snow_" + prop.Name : prop.Name;
                    
                    JArray positions = JArray.Parse(prop.Value.ToString());
                    
                    List<TilePosition> tilePositions = new List<TilePosition>();
                    
                    foreach (JToken position in positions.Children())
                    {
                        TilePosition pos;
                        
                        pos.x = int.Parse(position["x"]?.ToString() ?? "0");
                        pos.y = int.Parse(position["y"]?.ToString() ?? "0");
                        pos.rot = RotationUtils.StringToDirection(position["rot"]?.ToString());
                        
                        tilePositions.Add(pos);
                    }
                    
                    tileData.Add(tileName, tilePositions);
                }
            }

            return tileData;
        }
    }
}