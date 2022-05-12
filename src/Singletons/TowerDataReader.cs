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
        public string AuraShaderMaterialName;
    }
    
    public class TowerDataReader : Node
    {
        private static string towersData =
            "{\"towers\":[{\"name\":\"RedA\",\"scene_name\":\"RedTowerA\",\"button_image\":\"tower_red_a\",\"cost\":1,\"attack_range\":1,\"damage\":1,\"attack_speed\":1,\"projectile_speed\":250},{\"name\":\"PurpleA\",\"scene_name\":\"PurpleTowerA\",\"button_image\":\"tower_purple_a\",\"cost\":5,\"attack_range\":5,\"damage\":4,\"attack_speed\":0.8,\"projectile_speed\":200},{\"name\":\"Crystal\",\"scene_name\":\"RedTowerCrystal\",\"button_image\":\"tower_crystal\",\"cost\":20,\"aura_effect_name\":\"as20_dmg30\",\"aura_range\":5,\"aura_attack_speed\":100,\"aura_damage\":100,\"aura_shader_material_name\":\"rainbow\"}]}";
        public Dictionary<string, TowerData> GetTowersData()
        {
            Dictionary<string, TowerData> towers = new Dictionary<string, TowerData>();
            
            //string path = ProjectSettings.GlobalizePath("res://assets/data/towers/towers.json");
            string jsonFileText = towersData; //System.IO.File.ReadAllText(path);
            
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
            towerData.AuraShaderMaterialName = tower["aura_shader_material_name"]?.ToString() ?? "";
 
            return towerData;
        }
    }
}