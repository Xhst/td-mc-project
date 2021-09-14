using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Utils;

using TileMap = Godot.TileMap;


namespace TowerDefenseMC.Levels
{
    public class LevelBuilder : Node2D
    {
        private const int TileLength = 128;
        private const int TileHeight = 64;

        private TileMap _tileMap;
        
        public override void _Ready()
        {
            _tileMap = GetNode<TileMap>("TileMap");

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
        
    }
}