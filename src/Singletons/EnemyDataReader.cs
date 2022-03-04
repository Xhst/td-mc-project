using System.Collections.Generic;

using Godot;

using Newtonsoft.Json.Linq;

namespace TowerDefenseMC.Singletons
{
    public struct EnemyData
    {
        public string Name;
        public string Image;
        public float Health;
        public float Speed;
        public float Damage;
        public int Feed;
    }
    
    public class EnemyDataReader : Node
    {
        private static string enemiesData = "{\"enemies\":[{\"name\":\"Green\",\"image\":\"enemy_ufoGreen\",\"health\":3,\"speed\":300,\"damage\":5,\"feed\":5},{\"name\":\"Red\",\"image\":\"enemy_ufoRed\",\"health\":3,\"speed\":300,\"damage\":5,\"feed\":5},{\"name\":\"Purple\",\"image\":\"enemy_ufoPurple\",\"health\":3,\"speed\":300,\"damage\":5,\"feed\":5},{\"name\":\"Yellow\",\"image\":\"enemy_ufoYellow\",\"health\":3,\"speed\":300,\"damage\":5,\"feed\":5}]}";

        public Dictionary<string, EnemyData> GetEnemiesData()
        {
            Dictionary<string, EnemyData> enemies = new Dictionary<string, EnemyData>();
            
            //string path = ProjectSettings.GlobalizePath("res://assets/data/enemies/enemies.json");
            string jsonFileText = enemiesData; //System.IO.File.ReadAllText(path);
            
            JObject json = JObject.Parse(jsonFileText);

            if (json["enemies"] is null) return enemies;

            foreach (JToken enemy in json["enemies"])
            {
                EnemyData enemyData = GetEnemyData(enemy);

                if (enemyData.Name == string.Empty) continue;
                
                enemies.Add(enemyData.Name, enemyData);
            }

            return enemies;
        }

        private EnemyData GetEnemyData(JToken enemy)
        {
            EnemyData enemyData;

            enemyData.Name = enemy["name"]?.ToString() ?? "";
            enemyData.Image = enemy["image"]?.ToString() ?? "enemy_ufoGreen";
            enemyData.Damage = float.Parse(enemy["damage"]?.ToString() ?? "1");
            enemyData.Health = float.Parse(enemy["health"]?.ToString() ?? "1");
            enemyData.Speed = float.Parse(enemy["speed"]?.ToString() ?? "100");
            enemyData.Feed = int.Parse(enemy["feed"]?.ToString() ?? "0");

            return enemyData;
        }
    }
}