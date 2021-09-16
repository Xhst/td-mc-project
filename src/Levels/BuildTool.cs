using System.Collections.Generic;

using Godot;


namespace TowerDefenseMC.Levels
{
    public class BuildTool
    {
        private readonly LevelTemplate _levelTemplate;
        
        private bool _buildMode = false;
        private bool _canBuild = false;
        private bool _inMenu = false;

        private Color _currentColor;
        private readonly Color _yellow = new Color(0.755123f, 0.800781f, 0.150146f, 0.564706f);
        private readonly Color _red = new Color(0.8f, 0.14902f, 0.14902f, 0.564706f);

        private readonly HashSet<Vector2> _tilesWithBuildings;
        private Vector2 _currentTile = new Vector2();

        private readonly Towers.Towers _towers = new Towers.Towers();
        private PackedScene _currentTower;
        
        private readonly Node2D _buildToolInterface;
        private readonly Sprite _towerPlaceholder;
        
        public BuildTool(LevelTemplate levelTemplate)
        {
            _levelTemplate = levelTemplate;
            _tilesWithBuildings = new HashSet<Vector2>();
            
            _buildToolInterface = _levelTemplate.GetNode<Node2D>("BuildToolInterface");
            _towerPlaceholder = _levelTemplate.GetNode<Sprite>("BuildToolInterface/TowerPlaceholder");
        }

        public void Process()
        {
            if (!_buildMode) return;
            
            UpdateBuildTool();
            
            if(Input.IsActionJustPressed("left_mouse_button"))
            {
                BuildTower();
            }

            if (!Input.IsActionJustPressed("right_mouse_button")) return;

            _buildMode = false;
            _buildToolInterface.Hide();
        }
        
        private void UpdateBuildTool()
        {
            Vector2 mousePos = _levelTemplate.GetGlobalMousePosition();
            _currentTile = _levelTemplate.TileMap.WorldToMap(mousePos);
            _buildToolInterface.Position = _levelTemplate.TileMap.MapToWorld(_currentTile);

            if((_levelTemplate.TileMap.GetCellv(_currentTile) == _levelTemplate.TileMap.TileSet.FindTileByName("tile") ||
                _levelTemplate.TileMap.GetCellv(_currentTile) == _levelTemplate.TileMap.TileSet.FindTileByName("snow_tile")) && _currentColor != _yellow)
            {
                _currentColor = _yellow;
                _canBuild = true;
                (_towerPlaceholder.Material as ShaderMaterial)?.SetShaderParam("current_color", _currentColor);
            }

            if(_tilesWithBuildings.Contains(_currentTile) || 
               _levelTemplate.TileMap.GetCellv(_currentTile) != _levelTemplate.TileMap.TileSet.FindTileByName("tile") &&
               _levelTemplate.TileMap.GetCellv(_currentTile) != _levelTemplate.TileMap.TileSet.FindTileByName("snow_tile") && _currentColor != _red)
            {
                _currentColor = _red;
                _canBuild = false;
                (_towerPlaceholder.Material as ShaderMaterial)?.SetShaderParam("current_color", _currentColor);
            }
        }

        private void BuildTower()
        {
            if (!_canBuild || _inMenu) return;

            _tilesWithBuildings.Add(_currentTile);
            
            Node newTower = _currentTower.Instance();
            ((Node2D) newTower).GlobalPosition = _levelTemplate.TileMap.MapToWorld(_currentTile);
            _levelTemplate.GetNode<YSort>("TowerContainer").AddChild(newTower);
        }

        public void OnSelectTowerButtonDown(string towerName)
        {
            _currentTower = _towers.GetTower2PackedScene()[towerName];
            _towerPlaceholder.Texture = _towers.GetTower2Texture()[towerName];
            _buildMode = true;
            _buildToolInterface.Show();
        }

        public void OnTowerButtonMouseEntered()
        {
            _inMenu = true;
        }

        public void OnTowerButtonMouseExited()
        {
            _inMenu = false;
        }
    }
}