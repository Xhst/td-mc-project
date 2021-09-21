using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Singletons;
using TowerDefenseMC.Towers;


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
        
        private PackedScene _currentTower;

        private readonly TowerTemplate _towerTemplate = new TowerTemplate();
        
        private readonly Node2D _buildToolInterface;
        private readonly Sprite _towerPlaceholder;
        private readonly Polygon2D _attackRange;
        
        private readonly Dictionary<string, TowerData> _towersData;
        
        public BuildTool(LevelTemplate levelTemplate)
        {
            _levelTemplate = levelTemplate;
            _tilesWithBuildings = new HashSet<Vector2>();
            
            _buildToolInterface = _levelTemplate.GetNode<Node2D>("BuildToolInterface");
            _towerPlaceholder = _levelTemplate.GetNode<Sprite>("BuildToolInterface/TowerPlaceholder");
            _attackRange = _levelTemplate.GetNode<Polygon2D>("BuildToolInterface/AttackRange");

            _attackRange.Color = _yellow;
            
            TowerDataReader tdr = _levelTemplate.GetNode<TowerDataReader>("/root/TowerDataReader");
            _towersData = tdr.GetTowersData();
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
                _attackRange.Show();
            }

            if(_tilesWithBuildings.Contains(_currentTile) || 
               _levelTemplate.TileMap.GetCellv(_currentTile) != _levelTemplate.TileMap.TileSet.FindTileByName("tile") &&
               _levelTemplate.TileMap.GetCellv(_currentTile) != _levelTemplate.TileMap.TileSet.FindTileByName("snow_tile") && _currentColor != _red)
            {
                _currentColor = _red;
                _canBuild = false;
                (_towerPlaceholder.Material as ShaderMaterial)?.SetShaderParam("current_color", _currentColor);
                _attackRange.Hide();
            }
        }

        private void BuildTower()
        {
            if (!_canBuild || _inMenu) return;

            _tilesWithBuildings.Add(_currentTile);
            
            Node newTower = _currentTower.Instance();
            ((Node2D) newTower).GlobalPosition = _levelTemplate.TileMap.MapToWorld(_currentTile);
            _levelTemplate.GetNode<YSort>("EntitiesContainer").AddChild(newTower);
        }

        private async void UpdateTowerPlaceHolder()
        {
            TowerTemplate currentTowerScene = (TowerTemplate) _currentTower.Instance();
            
            Viewport previewViewport = _buildToolInterface.GetNode<Viewport>("Viewport");
            previewViewport.AddChild(currentTowerScene);

            await _levelTemplate.ToSignal(_levelTemplate.GetTree(), "idle_frame");
            await _levelTemplate.ToSignal(_levelTemplate.GetTree(), "idle_frame");

            ImageTexture imageTexture = new ImageTexture();
            Image image = new Image();
            
            image.CopyFrom(previewViewport.GetTexture().GetData());
            imageTexture.CreateFromImage(image);
            
            previewViewport.RemoveChild(currentTowerScene);
            
            _towerPlaceholder.Texture = imageTexture;
        }
        
        public void OnSelectTowerButtonDown(string towerName)
        {
            if (!_towersData.TryGetValue(towerName, out TowerData towerData)) return;

            _currentTower = ResourceLoader.Load<PackedScene>($"res://scenes/towers/{ towerData.SceneName }.tscn");
            
            UpdateTowerPlaceHolder();
            
            _attackRange.Polygon = TowerTemplate.GetRangeShapePoints(towerData.AttackRange);
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