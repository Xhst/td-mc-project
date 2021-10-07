using System.Collections.Generic;

using Godot;

using Newtonsoft.Json.Linq;

using TowerDefenseMC.Utils;


namespace TowerDefenseMC.Singletons
{

    public struct LevelData
    {
        public bool IsSnowy;
        public Dictionary<string, List<TilePosition>> Tiles;
        public List<List<Vector2>> EnemyPathsPoints;
        public List<List<Vector2>> RiversPoints;
        public List<WaveData> Waves;
        public int StartHealth;
        public int StartCrystals;
    }
    public struct TilePosition
    {
        public int X;
        public int Y;
        public Rotation Rot;
    }

    public struct WaveData
    {
        public float WaitTime;
        public List<WaveEnemyGroupData> WaveEnemies;
    }
    
    public struct WaveEnemyGroupData
    {
        public string Name;
        public int Amount;
        public int Path;
    }
    
    public class LevelDataReader : Node
    {
        public LevelData GetLevelData(int level)
        {
            LevelData levelData;
            
            levelData.Tiles =  new Dictionary<string, List<TilePosition>>();
            levelData.EnemyPathsPoints = new List<List<Vector2>>();
            levelData.RiversPoints = new List<List<Vector2>>();
            levelData.Waves = new List<WaveData>();
            levelData.StartHealth = 10;
            levelData.StartCrystals = 10;
            
            string path = ProjectSettings.GlobalizePath($"res://assets/data/levels/level{ level }.json");
            string jsonFileText = System.IO.File.ReadAllText(path);
            
            JObject json = JObject.Parse(jsonFileText);

            bool isSnowy = json["snowy"]?.ToString() == bool.TrueString;
            levelData.IsSnowy = isSnowy;
            
            if (json["tiles"] is null) return levelData;

            levelData.Tiles = GetTilesData(json["tiles"]);
            levelData.EnemyPathsPoints = GetPointsListData(json["enemy_paths"]);
            levelData.RiversPoints = GetPointsListData(json["rivers"]);
            levelData.Waves = GetWavesData(json["enemies_waves"]);
            levelData.StartHealth = int.Parse(json["start_health"]?.ToString() ?? "10");
            levelData.StartCrystals = int.Parse(json["start_crystals"]?.ToString() ?? "10");

            return levelData;
        }

        private Dictionary<string, List<TilePosition>> GetTilesData(JToken jsonTiles)
        {
            Dictionary<string, List<TilePosition>> tileData = new Dictionary<string, List<TilePosition>>();
            
            JArray tiles = JArray.Parse(jsonTiles.ToString());

            foreach (JObject content in tiles.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string tileName = prop.Name;
                    
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

        private List<List<Vector2>> GetPointsListData(JToken pointsListData)
        {
            List<List<Vector2>> pointsList = new List<List<Vector2>>();

            foreach (JToken list in pointsListData)
            {
                List<Vector2> points = new List<Vector2>();
                
                foreach (JToken point in list)
                {

                    int x = int.Parse(point["x"]?.ToString() ?? "0");
                    int y = int.Parse(point["y"]?.ToString() ?? "0");

                    points.Add(new Vector2(x,y));
                }
                
                pointsList.Add(points);
            }
            
            return pointsList;
        }

        private List<WaveData> GetWavesData(JToken jsonWavesData)
        {
            List<WaveData> wavesData = new List<WaveData>();

            foreach (JToken wave in jsonWavesData)
            {
                WaveData waveData;
                
                waveData.WaitTime = float.Parse(wave["wait_time"]?.ToString() ?? "5");
                waveData.WaveEnemies = GetWaveEnemiesData(wave["enemies"]);
                
                wavesData.Add(waveData);
            }

            return wavesData;
        }

        private List<WaveEnemyGroupData> GetWaveEnemiesData(JToken jsonWaveEnemiesData)
        {
            List<WaveEnemyGroupData> waveEnemiesData = new List<WaveEnemyGroupData>();

            foreach (JToken enemiesGroup in jsonWaveEnemiesData)
            {
                WaveEnemyGroupData waveEnemyGroupData;

                waveEnemyGroupData.Name = enemiesGroup["name"]?.ToString() ?? "";
                waveEnemyGroupData.Amount = int.Parse(enemiesGroup["amount"]?.ToString() ?? "0");
                waveEnemyGroupData.Path = int.Parse(enemiesGroup["path"]?.ToString() ?? "0");
                
                if (waveEnemyGroupData.Name == "" || waveEnemyGroupData.Amount == 0) continue;
                
                waveEnemiesData.Add(waveEnemyGroupData);
            }

            return waveEnemiesData;
        }
    }
}