using System;
using System.Collections.Generic;

using Godot;

using Newtonsoft.Json.Linq;

using TowerDefenseMC.Utils;


namespace TowerDefenseMC.Levels
{

    public struct LevelData
    {
        public bool IsSnowy;
        public Dictionary<string, List<TilePosition>> Tiles;
    }
    public struct TilePosition
    {
        public int X;
        public int Y;
        public Rotation Rot;
    }
    
    public class LevelDataReader : Node
    {
        public LevelData GetLevelData(int level)
        {
            LevelData levelData;
            
            levelData.Tiles =  new Dictionary<string, List<TilePosition>>();
            
            string path = ProjectSettings.GlobalizePath($"res://assets/levels/level{level}.json");
            string jsonFileText = System.IO.File.ReadAllText(path);
            
            JObject json = JObject.Parse(jsonFileText);

            bool isSnowy = json["snowy"]?.ToString() == bool.TrueString;
            levelData.IsSnowy = isSnowy;
            
            if (json["tiles"] == null) return levelData;

            levelData.Tiles = GetTilesData(isSnowy, json["tiles"]);
            

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
                        
                        pos.X = int.Parse(position["x"]?.ToString() ?? "0");
                        pos.Y = int.Parse(position["y"]?.ToString() ?? "0");
                        pos.Rot = RotationUtils.StringToDirection(position["rot"]?.ToString());
                        
                        tilePositions.Add(pos);
                    }
                    
                    tileData.Add(tileName, tilePositions);
                }
            }

            return tileData;
        }
    }
}