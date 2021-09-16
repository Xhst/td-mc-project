using System.Collections.Generic;

using Godot;


namespace TowerDefenseMC.Towers
{
    public class Towers : Node
    {
        private readonly Texture _uiRedTower = ResourceLoader.Load<Texture>("res://assets/img/towers/towerRound_sampleC_N.png");
        private readonly Texture _uiPurpleTower = ResourceLoader.Load<Texture>("res://assets/img/towers/towerSquare_sampleF_N.png");

        private readonly PackedScene _redTower = ResourceLoader.Load<PackedScene>("res://tower/RedTower.tscn");
        private readonly PackedScene _purpleTower = ResourceLoader.Load<PackedScene>("res://tower/PurpleTower.tscn");

        public Dictionary<string, Texture> GetTower2Texture()
        {
            return new Dictionary<string, Texture>(){
                {"RedTower", _uiRedTower},
                {"PurpleTower", _uiPurpleTower}
            };
        }

        public Dictionary<string, PackedScene> GetTower2PackedScene()
        {
            return new Dictionary<string, PackedScene>(){
                {"RedTower", _redTower},
                {"PurpleTower", _purpleTower}
            };
        }
    }
}