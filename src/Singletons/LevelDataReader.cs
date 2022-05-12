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
        private Dictionary<int,string> levels = new Dictionary<int, string>();

        public LevelDataReader()
        {
            levels.Add(0, "{\"snowy\":false,\"start_health\":100,\"start_crystals\":10,\"tiles\":[{\"tile_treeDouble\":[{\"x\":10,\"y\":2},{\"x\":20,\"y\":-1,\"rot\":\"EW\"},{\"x\":20,\"y\":-4,\"rot\":\"E\"}]}],\"rivers\":[[{\"x\":5,\"y\":-6},{\"x\":30,\"y\":-15}]],\"enemy_paths\":[[{\"x\":7,\"y\":5},{\"x\":17,\"y\":5},{\"x\":17,\"y\":-2}]],\"enemies_waves\":[{\"wait_time\":10,\"enemies\":[{\"name\":\"Yellow\",\"amount\":7},{\"path\":1,\"name\":\"Green\",\"amount\":2},{\"path\":1,\"name\":\"Purple\",\"amount\":2}]},{\"wait_time\":15,\"enemies\":[{\"name\":\"Yellow\",\"amount\":3},{\"name\":\"Green\",\"amount\":3},{\"path\":1,\"name\":\"Purple\",\"amount\":4}]}]}");
            levels.Add(1, "{\"snowy\":false,\"start_health\":100,\"start_crystals\":100,\"tiles\":[{\"tile_treeDouble\":[{\"x\":15,\"y\":2},{\"x\":20,\"y\":-1,\"rot\":\"EW\"},{\"x\":20,\"y\":-4,\"rot\":\"E\"}]}],\"rivers\":[[{\"x\":5,\"y\":-5},{\"x\":15,\"y\":-5},{\"x\":17,\"y\":17}]],\"enemy_paths\":[[{\"x\":12,\"y\":2},{\"x\":22,\"y\":4}],[{\"x\":17,\"y\":-3},{\"x\":19,\"y\":8}]],\"enemies_waves\":[{\"wait_time\":10,\"enemies\":[{\"name\":\"Yellow\",\"amount\":6},{\"path\":1,\"name\":\"Green\",\"amount\":2},{\"path\":1,\"name\":\"Purple\",\"amount\":2}]},{\"wait_time\":15,\"enemies\":[{\"name\":\"Yellow\",\"amount\":3},{\"name\":\"Green\",\"amount\":3},{\"path\":1,\"name\":\"Purple\",\"amount\":4}]}]}");
            levels.Add(2, "{\"snowy\":false,\"start_health\":100,\"start_crystals\":100,\"tiles\":[{\"tile_treeDouble\":[{\"x\":15,\"y\":2},{\"x\":20,\"y\":-1,\"rot\":\"EW\"},{\"x\":20,\"y\":-4,\"rot\":\"E\"}]}],\"rivers\":[[{\"x\":5,\"y\":-5},{\"x\":20,\"y\":-7}]],\"enemy_paths\":[[{\"x\":2,\"y\":0},{\"x\":17,\"y\":0}],[{\"x\":8,\"y\":-4},{\"x\":10,\"y\":5}]],\"enemies_waves\":[{\"wait_time\":10,\"enemies\":[{\"name\":\"Yellow\",\"amount\":10},{\"path\":1,\"name\":\"Green\",\"amount\":2},{\"path\":1,\"name\":\"Purple\",\"amount\":2}]},{\"wait_time\":15,\"enemies\":[{\"name\":\"Yellow\",\"amount\":3},{\"name\":\"Green\",\"amount\":3},{\"path\":1,\"name\":\"Purple\",\"amount\":4}]}]}");
            
            for (int i = 3; i <= 9; i++)
            {
                levels.Add(i, "{}");
            }
        }

        public int LevelsCount()
        {
            return levels.Count;
        }

        public LevelData GetLevelData(int level)
        {
            LevelData levelData;
            
            levelData.Tiles =  new Dictionary<string, List<TilePosition>>();
            levelData.EnemyPathsPoints = new List<List<Vector2>>();
            levelData.RiversPoints = new List<List<Vector2>>();
            levelData.Waves = new List<WaveData>();
            levelData.StartHealth = 10;
            levelData.StartCrystals = 10;
            
            //string path = ProjectSettings.GlobalizePath($"res://assets/data/levels/level{ level }.json");
            levels.TryGetValue(level, out string jsonFileText); //string jsonFileText = System.IO.File.ReadAllText(path);
            
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