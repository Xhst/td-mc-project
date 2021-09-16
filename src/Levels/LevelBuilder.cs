
using System.Collections.Generic;

using Godot;

using AStar = TowerDefenseMC.Utils.AStar;
using TileMap = Godot.TileMap;


namespace TowerDefenseMC.Levels
{
    public class LevelBuilder : Node2D
    {
        private bool build_mode = false;
        private bool can_build = false;
        private bool in_menu = false;

        private Color current_color;
        private Color yellow = new Color(0.755123f, 0.800781f, 0.150146f, 0.564706f);
        private Color red = new Color(0.8f, 0.14902f, 0.14902f, 0.564706f);

        private Vector2 current_tile = new Vector2();

        private Towers towers = new Towers();
        private PackedScene current_tower;

        private const int TileLength = 128;
        private const int TileHeight = 64;

        private TileMap _tileMap;
        private Node2D _buildTool;
        private Sprite _buildInterface;

        private TileMap _tileMap;

        private TerrainBuilder _terrainBuilder;

        private List<List<Vector2>> _paths;
        private List<List<Vector2>> _rivers;

        public override void _Ready()
        {
            _tileMap = GetNode<TileMap>("TileMap");

            _buildTool = GetNode<Node2D>("Build_Tool");
            _buildInterface = GetNode<Sprite>("Build_Tool/BuildInterface");
            
            _paths = new List<List<Vector2>>();

            LevelDataReader ldr = GetNode<LevelDataReader>("/root/LevelDataReader");
            LevelData levelData = ldr.GetLevelData(1);
            
            _paths = CalculateTilesInPointsLists(levelData.EnemyPathsPoints);
            _rivers = CalculateTilesInPointsLists(levelData.RiversPoints);

            _terrainBuilder = new TerrainBuilder(levelData.IsSnowy, _tileMap);
            _terrainBuilder.FillViewPortWithTile(GetViewSize());
            _terrainBuilder.DrawCustomTiles(levelData.Tiles);
            _terrainBuilder.DrawPathsAndRivers(_paths, _rivers);
        }

        public override void _PhysicsProcess(float delta)
        {
            if(build_mode)
            {
                UpdateBuildTool();
                if(Input.IsActionJustPressed("left_mouse_button"))
                {
                    BuildTower();
                }
                if(Input.IsActionJustPressed("right_mouse_button"))
                {
                    build_mode = false;
                    _buildTool.Hide();
                }
            }
        }

        private Vector2 GetViewSize()
        {
            Transform2D canvasTransform = GetCanvasTransform();
            return GetViewportRect().Size / canvasTransform.Scale;
        }

        private void UpdateBuildTool()
        {
            Vector2 mouse_pos = GetGlobalMousePosition();
            current_tile = _tileMap.WorldToMap(mouse_pos);
            _buildTool.Position = _tileMap.MapToWorld(current_tile);

            if(_tileMap.GetCellv(current_tile) == _tileMap.TileSet.FindTileByName("tile") && current_color != yellow)
            {
                current_color = yellow;
                can_build = true;
                (_buildInterface.Material as ShaderMaterial).SetShaderParam("current_color", current_color);
            }

            if(_tileMap.GetCellv(current_tile) != _tileMap.TileSet.FindTileByName("tile") && current_color != red)
            {
                current_color = red;
                can_build = false;
                (_buildInterface.Material as ShaderMaterial).SetShaderParam("current_color", current_color);
            }
        }

        private void BuildTower()
        {
            if(can_build && !in_menu)
            {
                _tileMap.SetCellv(current_tile, 29);
                Node new_tower = current_tower.Instance();
                (new_tower as Node2D).GlobalPosition = _tileMap.MapToWorld(current_tile);
                GetNode<YSort>("TowerContainer").AddChild(new_tower);
            }
        }

        public void _on_Select_Tower_button_down(string tower_name)
        {
            current_tower = towers.GetTower2PackedScene()[tower_name];
            _buildInterface.Texture = towers.GetTower2Texture()[tower_name];
            build_mode = true;
            _buildTool.Show();
        }

        public void _on_Tower_Button_mouse_entered()
        {
            in_menu = true;
        }

        public void _on_Tower_Button_mouse_exited()
        {
            in_menu = false;
        }
    
    }
}