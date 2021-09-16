using System.Collections.Generic;

using Godot;


namespace TowerDefenseMC.Levels
{
    public class Towers : Node
    {
        private Texture ui_red_tower = ResourceLoader.Load<Texture>("res://assets/img/towers/towerRound_sampleC_N.png");
        private Texture ui_purple_tower = ResourceLoader.Load<Texture>("res://assets/img/towers/towerSquare_sampleF_N.png");

        private PackedScene red_tower = ResourceLoader.Load<PackedScene>("res://tower/Red_Tower.tscn");
        private PackedScene purple_tower = ResourceLoader.Load<PackedScene>("res://tower/Purple_Tower.tscn");

        public Dictionary<string, Texture> GetTower2Texture()
        {
            return new Dictionary<string, Texture>(){
                {"Red_Tower", ui_red_tower},
                {"Purple_Tower", ui_purple_tower}
            };
        }

        public Dictionary<string, PackedScene> GetTower2PackedScene()
        {
            return new Dictionary<string, PackedScene>(){
                {"Red_Tower", red_tower},
                {"Purple_Tower", purple_tower}
            };
        }
    }
}