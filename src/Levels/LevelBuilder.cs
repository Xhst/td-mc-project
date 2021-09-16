
using System.Collections.Generic;

using Godot;

using AStar = TowerDefenseMC.Utils.AStar;
using TileMap = Godot.TileMap;


namespace TowerDefenseMC.Levels
{
    public class LevelBuilder : Node2D
    {
        private bool _buildMode = false;
        private bool _canBuild = false;
        private bool _inMenu = false;

        private Color _currentColor;
        private readonly Color _yellow = new Color(0.755123f, 0.800781f, 0.150146f, 0.564706f);
        private readonly Color _red = new Color(0.8f, 0.14902f, 0.14902f, 0.564706f);

        private HashSet<Vector2> _tilesWithBuildings;
        private Vector2 _currentTile = new Vector2();

        private readonly Towers _towers = new Towers();
        private PackedScene _currentTower;

        private TileMap _tileMap;
        
        private Node2D _buildTool;
        private Sprite _buildInterface;
        

        private TerrainBuilder _terrainBuilder;

        private List<List<Vector2>> _paths;
        private List<List<Vector2>> _rivers;

        public override void _Ready()
        {
            _tileMap = GetNode<TileMap>("TileMap");

            _buildTool = GetNode<Node2D>("Build_Tool");
            _buildInterface = GetNode<Sprite>("Build_Tool/BuildInterface");
            
            _tilesWithBuildings = new HashSet<Vector2>();
            
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
            if(_buildMode)
            {
                UpdateBuildTool();
                if(Input.IsActionJustPressed("left_mouse_button"))
                {
                    BuildTower();
                }
                if(Input.IsActionJustPressed("right_mouse_button"))
                {
                    _buildMode = false;
                    _buildTool.Hide();
                }
            }
        }

        private Vector2 GetViewSize()
        {
            Transform2D canvasTransform = GetCanvasTransform();
            return GetViewportRect().Size / canvasTransform.Scale;
        }
        
        private List<List<Vector2>> CalculateTilesInPointsLists(List<List<Vector2>> pointsLists)
        {
            List<List<Vector2>> listsOfTiles = new List<List<Vector2>>();
            
            AStar aStar = new AStar(false);

            foreach (List<Vector2> list in pointsLists)
            {
                List<Vector2> tiles = new List<Vector2>();

                for (int i = 1; i < list.Count; i++)
                {
                    tiles.AddRange(aStar.FindPath(list[i - 1], list[i]));
                    aStar.Clear();
                }

                listsOfTiles.Add(tiles);
            }

            return listsOfTiles;
        }


        private void UpdateBuildTool()
        {
            Vector2 mousePos = GetGlobalMousePosition();
            _currentTile = _tileMap.WorldToMap(mousePos);
            _buildTool.Position = _tileMap.MapToWorld(_currentTile);

            if((_tileMap.GetCellv(_currentTile) == _tileMap.TileSet.FindTileByName("tile") ||
               _tileMap.GetCellv(_currentTile) == _tileMap.TileSet.FindTileByName("snow_tile")) && _currentColor != _yellow)
            {
                _currentColor = _yellow;
                _canBuild = true;
                (_buildInterface.Material as ShaderMaterial)?.SetShaderParam("current_color", _currentColor);
            }

            if(_tilesWithBuildings.Contains(_currentTile) || 
               _tileMap.GetCellv(_currentTile) != _tileMap.TileSet.FindTileByName("tile") &&
               _tileMap.GetCellv(_currentTile) != _tileMap.TileSet.FindTileByName("snow_tile") && _currentColor != _red)
            {
                _currentColor = _red;
                _canBuild = false;
                (_buildInterface.Material as ShaderMaterial)?.SetShaderParam("current_color", _currentColor);
            }
        }

        private void BuildTower()
        {
            if (!_canBuild || _inMenu) return;

            _tilesWithBuildings.Add(_currentTile);
            
            Node newTower = _currentTower.Instance();
            ((Node2D) newTower).GlobalPosition = _tileMap.MapToWorld(_currentTile);
            GetNode<YSort>("TowerContainer").AddChild(newTower);
        }

        public void _on_Select_Tower_button_down(string towerName)
        {
            _currentTower = _towers.GetTower2PackedScene()[towerName];
            _buildInterface.Texture = _towers.GetTower2Texture()[towerName];
            _buildMode = true;
            _buildTool.Show();
        }

        public void _on_Tower_Button_mouse_entered()
        {
            _inMenu = true;
        }

        public void _on_Tower_Button_mouse_exited()
        {
            _inMenu = false;
        }
    
    }
}