using System.Collections.Generic;

using Godot;


namespace TowerDefenseMC.Towers
{
    public class Towers
    {
        private readonly Texture _uiRedTower = ResourceLoader.Load<Texture>("res://assets/img/towers/towerRound_sampleC.png");
        private readonly Texture _uiPurpleTower = ResourceLoader.Load<Texture>("res://assets/img/towers/towerSquare_sampleF.png");

        private readonly PackedScene _redTower = ResourceLoader.Load<PackedScene>("res://tower/RedTowerA.tscn");
        private readonly PackedScene _purpleTower = ResourceLoader.Load<PackedScene>("res://tower/PurpleTowerA.tscn");

        public Dictionary<string, Texture> GetTower2Texture()
        {
            return new Dictionary<string, Texture>() {
                {"RedTower", _uiRedTower},
                {"PurpleTower", _uiPurpleTower}
            };
        }

        public Dictionary<string, PackedScene> GetTower2PackedScene()
        {
            return new Dictionary<string, PackedScene>() {
                {"RedTower", _redTower},
                {"PurpleTower", _purpleTower}
            };
        }

        public Dictionary<string, int> GetTower2AttackRange()
        {
            return new Dictionary<string, int>() {
                {"RedTower", 1},
                {"PurpleTower", 2}
            };
        }
    }
}