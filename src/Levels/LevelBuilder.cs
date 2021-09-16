using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Utils;

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

        public override void _Ready()
        {
            _tileMap = GetNode<TileMap>("TileMap");
            _buildTool = GetNode<Node2D>("Build_Tool");
            _buildInterface = GetNode<Sprite>("Build_Tool/BuildInterface");

            LevelDataReader ldr = GetNode<LevelDataReader>("/root/LevelDataReader");
            LevelData levelData = ldr.GetLevelData(1);

            int fillTileId = _tileMap.TileSet.FindTileByName(levelData.IsSnowy ? "snow_tile" : "tile");
            
            FillViewPortWithTile(fillTileId);

            foreach (KeyValuePair<string, List<TilePosition>> tile in levelData.Tiles)
            {
                foreach (TilePosition tilePosition in tile.Value)
                {
                    int tileId = FindTileByNameTryingRotation(tile.Key, tilePosition.Rot);
                    
                    _tileMap.SetCell(tilePosition.X, tilePosition.Y, tileId);
                }
            }
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

        private int FindTileByNameTryingRotation(string name, Rotation rotation)
        {
            int tileId = _tileMap.TileSet.FindTileByName(name);

            if (tileId != -1) return tileId;
            
            tileId = _tileMap.TileSet.FindTileByName(name + "_" + rotation.ToStringShort());
            
            if (tileId != -1) return tileId;
            
            return _tileMap.TileSet.FindTileByName(name + "_" + rotation.ToStringPair());
        }

        private Vector2 GetViewSize()
        {
            Transform2D canvasTransform = GetCanvasTransform();
            return GetViewportRect().Size / canvasTransform.Scale;
        }

        private void FillViewPortWithTile(int tileId)
        {
            Vector2 viewSize = GetViewSize();
            
            int length = (int) viewSize.x  / TileLength;
            int height = (int) viewSize.y / TileHeight;

            Vector2 dx = new Vector2(1, -1);
            Vector2 dy = new Vector2(1, 1);

            for (int x = 0; x <= length; x++)
            {
                for (int y = -1; y <= height + 1; y++)
                {
                    Vector2 newTilePos = x * dx + y * dy;
                    _tileMap.SetCellv(newTilePos, tileId);

                    if (x < length)
                    {
                        _tileMap.SetCell((int) newTilePos.x + 1, (int) newTilePos.y, tileId);
                    }
                }
            }
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