using System.Collections.Generic;

using Godot;

using Newtonsoft.Json.Linq;


namespace TowerDefenseMC.Singletons
{
    public struct TowerData
    {
        public string Name;
        public string SceneName;
        public string ButtonImage;
        public int Cost;
        public int AttackRange;
        public float Damage;
        public float AttackSpeed;
        public float ProjectileSpeed;
        public string AuraEffectName;
        public int AuraRange;
        public float AuraDamage;
        public float AuraAttackSpeed;
    }
    
    public class TowerDataReader : Node
    {

        public Dictionary<string, TowerData> GetTowersData()
        {
            Dictionary<string, TowerData> towers = new Dictionary<string, TowerData>();
            
            string path = ProjectSettings.GlobalizePath("res://assets/data/towers/towers.json");
            string jsonFileText = System.IO.File.ReadAllText(path);
            
            JObject json = JObject.Parse(jsonFileText);

            if (json["towers"] is null) return towers;

            foreach (JToken tower in json["towers"])
            {
                TowerData towerData = GetTowerData(tower);

                if (towerData.Name == string.Empty) continue;
                
                towers.Add(towerData.Name, towerData);
            }

            return towers;
        }

        private TowerData GetTowerData(JToken tower)
        {
            TowerData towerData;

            towerData.Name = tower["name"]?.ToString() ?? "";
            towerData.SceneName = tower["scene_name"]?.ToString() ?? "RedTowerA";
            towerData.ButtonImage = tower["button_image"]?.ToString() ?? "tower_red_a";
            towerData.Cost = int.Parse(tower["cost"]?.ToString() ?? "0");
            towerData.AttackRange = int.Parse(tower["attack_range"]?.ToString() ?? "0");
            towerData.Damage = float.Parse(tower["damage"]?.ToString() ?? "0");
            towerData.AttackSpeed = float.Parse(tower["attack_speed"]?.ToString() ?? "0");
            towerData.ProjectileSpeed = float.Parse(tower["projectile_speed"]?.ToString() ?? "0");
            towerData.AuraEffectName = tower["aura_effect_name"]?.ToString() ?? "";
            towerData.AuraRange = int.Parse(tower["aura_range"]?.ToString() ?? "0");
            towerData.AuraDamage = float.Parse(tower["aura_damage"]?.ToString() ?? "0");
            towerData.AuraAttackSpeed = float.Parse(tower["aura_attack_speed"]?.ToString() ?? "0");
 
            return towerData;
        }
    }
}