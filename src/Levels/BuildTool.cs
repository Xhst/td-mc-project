using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Singletons;
using TowerDefenseMC.Towers;
using TowerDefenseMC.UserInterface.Statistics;
using TowerDefenseMC.UserInterface.Shop;


namespace TowerDefenseMC.Levels
{
    public class BuildTool
    {
        private readonly LevelTemplate _levelTemplate;
        
        private bool _buildMode = false;
        private bool _canBuild = false;
        private bool _inMenu = false;

        private Color _currentColor;

        private readonly Color _buildAllowedColor = new Color(1f, 1f, 1f, 0.7f);
        private readonly Color _buildNotAllowedColor = new Color(0.9f, 0.2f, 0.2f, 0.7f);
        private readonly Color _attackRangeColor = new Color(1f, 0.7f, 0f, 0.3f);
        private readonly Color _auraRangeColor = new Color(0.2f, 0.75f, 0.8f, 0.3f);
        
        private Vector2 _currentTile = new Vector2();

        private string _currentTowerName;
        private int _towerCost;

        private PackedScene _currentTower;

        private readonly Node2D _buildToolInterface;
        private readonly Sprite _towerPlaceholder;
        private readonly Polygon2D _attackRange;
        private readonly Polygon2D _auraRange;
        
        private readonly StatisticsInterface _statisticsInterface;
        private readonly ShopInterface _shopInterface;

        private readonly Dictionary<string, TowerData> _towersData;
        
        public BuildTool(LevelTemplate levelTemplate)
        {
            _levelTemplate = levelTemplate;

            _buildToolInterface = _levelTemplate.GetNode<Node2D>("BuildToolInterface");
            _towerPlaceholder = _levelTemplate.GetNode<Sprite>("BuildToolInterface/TowerPlaceholder");
            _attackRange = _levelTemplate.GetNode<Polygon2D>("BuildToolInterface/AttackRange");
            _auraRange = _levelTemplate.GetNode<Polygon2D>("BuildToolInterface/AuraRange");

            _attackRange.Color = _attackRangeColor;
            _auraRange.Color = _auraRangeColor;
            
            TowerDataReader tdr = _levelTemplate.GetNode<TowerDataReader>("/root/TowerDataReader");
            _towersData = tdr.GetTowersData();
            
            _shopInterface = _levelTemplate.GetNode<ShopInterface>("UI/ShopInterface");
            _statisticsInterface = _levelTemplate.GetNode<StatisticsInterface>("UI/StatisticsInterface");

            _shopInterface.LoadButtons(_towersData);
            _shopInterface.SetPlayer(_levelTemplate.Player);
        }

        public void Process()
        {
            if (!_buildMode) return;
            
            UpdateBuildTool();
            
            if(Input.IsActionJustPressed("left_mouse_button"))
            {
                BuildTower();
            }

            if (!Input.IsActionJustPressed("right_mouse_button") && CanBuildAnotherTower()) return;

            _buildMode = false;
            _buildToolInterface.Hide();
        }
        
        private void UpdateBuildTool()
        {
            Vector2 mousePos = _levelTemplate.GetGlobalMousePosition();
            _currentTile = _levelTemplate.TileMap.WorldToMap(mousePos);
            _buildToolInterface.Position = _levelTemplate.TileMap.MapToWorld(_currentTile);

            if ((_levelTemplate.TileMap.GetCellv(_currentTile) == _levelTemplate.TileMap.TileSet.FindTileByName("tile") ||
                _levelTemplate.TileMap.GetCellv(_currentTile) == _levelTemplate.TileMap.TileSet.FindTileByName("snow_tile")) && _currentColor != _buildAllowedColor)
            {
                _currentColor = _buildAllowedColor;
                _canBuild = true;
                (_towerPlaceholder.Material as ShaderMaterial)?.SetShaderParam("current_color", _currentColor);
                _attackRange.Show();
                _auraRange.Show();
            }

            if (_levelTemplate.TileHasTower(_currentTile) || 
               _levelTemplate.TileMap.GetCellv(_currentTile) != _levelTemplate.TileMap.TileSet.FindTileByName("tile") &&
               _levelTemplate.TileMap.GetCellv(_currentTile) != _levelTemplate.TileMap.TileSet.FindTileByName("snow_tile") && _currentColor != _buildNotAllowedColor)
            {
                _currentColor = _buildNotAllowedColor;
                _canBuild = false;
                (_towerPlaceholder.Material as ShaderMaterial)?.SetShaderParam("current_color", _currentColor);
                _attackRange.Hide();
                _auraRange.Hide();
            }
        }

        private void BuildTower()
        {
            if (!_canBuild || _inMenu) return;
            if (!_towersData.TryGetValue(_currentTowerName, out TowerData towerData)) return;

            TowerTemplate newTower = (TowerTemplate) _currentTower.Instance();
            newTower.GlobalPosition = _levelTemplate.TileMap.MapToWorld(_currentTile);
            newTower.Init(towerData, _levelTemplate, _currentTile);
            
            _levelTemplate.GetNode<YSort>("EntitiesContainer").AddChild(newTower);
            _levelTemplate.AddTowerOnTile(_currentTile, newTower);
            newTower.OnPlace();

            _levelTemplate.Player.Crystals -= _towerCost;
            _shopInterface.TowerBuilt();
        }

        private bool CanBuildAnotherTower()
        {
            return _levelTemplate.Player.Crystals - _towerCost >= 0;
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

        public void OnSelectTowerButtonDown(string towerName, int towerCost)
        {
            if (!_towersData.TryGetValue(towerName, out TowerData towerData)) return;

            _currentTowerName = towerName;
            _currentTower = ResourceLoader.Load<PackedScene>($"scenes/towers/{ towerData.SceneName }.tscn");
            
            UpdateTowerPlaceHolder();

            if (towerData.AttackRange > 0)
            {
                _attackRange.Polygon = TowerTemplate.GetRangeShapePoints(towerData.AttackRange);
            }

            if (towerData.AuraRange > 0)
            {
                _auraRange.Polygon = TowerTemplate.GetRangeShapePoints(towerData.AuraRange);
            }
            
            _buildMode = true;
            _buildToolInterface.Show();

            _towerCost = towerCost;
        }

        public void OnTowerButtonMouseEntered()
        {
            _inMenu = true;
        }

        public void OnTowerButtonMouseExited()
        {
            _inMenu = false;
        }

        public void TowerStatistics(string towerName, TowerTemplate tower)
        {
            if (!_towersData.TryGetValue(towerName, out TowerData towerData)) return;

            _statisticsInterface.SetTowerTemplate(tower);
            _statisticsInterface.SetTowerStatisticValues(towerData);
            _statisticsInterface.Show();
        }

        public void HideTowerStatistics()
        {
            _statisticsInterface.Hide();
        }
    }
}