using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Singletons;
using TowerDefenseMC.Utils;


namespace TowerDefenseMC.Levels
{
    public class TerrainBuilder
    {
        private const int TileLength = 128;
        private const int TileHeight = 64;

        private readonly TileMap _tileMap;
        private readonly bool _isSnowy;
        
        private readonly int _fillTileId;

        public TerrainBuilder(bool isSnowy, TileMap tileMap)
        {
            _isSnowy = isSnowy;
            _tileMap = tileMap;

            _fillTileId = _tileMap.TileSet.FindTileByName(_isSnowy ? "snow_tile" : "tile");
        }

        public void FillViewPortWithTile(Vector2 viewSize)
        {
            int length = (int) viewSize.x  / TileLength;
            int height = (int) viewSize.y / TileHeight;

            Vector2 dx = new Vector2(1, -1);
            Vector2 dy = new Vector2(1, 1);

            for (int x = 0; x <= length; x++)
            {
                for (int y = -1; y <= height + 1; y++)
                {
                    Vector2 newTilePos = x * dx + y * dy;
                    _tileMap.SetCellv(newTilePos, _fillTileId);

                    if (x < length)
                    {
                        _tileMap.SetCell((int) newTilePos.x + 1, (int) newTilePos.y, _fillTileId);
                    }
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
        
        public void DrawCustomTiles(Dictionary<string, List<TilePosition>> tiles)
        {
            foreach (KeyValuePair<string, List<TilePosition>> tile in tiles)
            {
                foreach (TilePosition tilePosition in tile.Value)
                {
                    string tileName = _isSnowy ? "snow_" + tile.Key : tile.Key;
                    
                    int tileId = FindTileByNameTryingRotation(tileName, tilePosition.Rot);
                    
                    _tileMap.SetCell(tilePosition.X, tilePosition.Y, tileId);
                }
            }
        }

        public void DrawPathsAndRivers(List<List<Vector2>> paths, List<List<Vector2>> rivers)
        {
            HashSet<Vector2> pathTiles = new HashSet<Vector2>();
            
            foreach (List<Vector2> path in paths)
            {
                foreach (Vector2 tile in path)
                {
                    pathTiles.Add(tile);
                }
            }
            
            HashSet<Vector2> riverTiles = new HashSet<Vector2>();
            
            foreach (List<Vector2> river in rivers)
            {
                foreach (Vector2 tile in river)
                {
                    riverTiles.Add(tile);
                }
            }
            
            DrawPaths(paths, pathTiles);
            DrawRivers(rivers, riverTiles, pathTiles);
        }
        private void DrawPaths(List<List<Vector2>> paths, HashSet<Vector2> pathTiles)
        {
            foreach (List<Vector2> path in paths)
            {
                foreach (Vector2 tile in path)
                {
                    int incidenceNumber = CalculateTileIncidencePathNumber(tile, pathTiles);
                    int tileId = GetPathTileIdFromIncidenceNumber(incidenceNumber);
                    
                    if (_isSnowy)
                    {
                        tileId += 123;
                    }
                    
                    _tileMap.SetCellv(tile, tileId);
                    
                }
            }
        }
        
        private void DrawRivers(List<List<Vector2>> rivers, HashSet<Vector2> riverTiles, HashSet<Vector2> pathTiles)
        {
            foreach (List<Vector2> river in rivers)
            {
                foreach (Vector2 tile in river)
                {
                    int incidenceNumber = CalculateTileIncidencePathNumber(tile, riverTiles);
                    int tileId = GetRiverTileIdFromIncidenceNumber(incidenceNumber);

                    if (pathTiles.Contains(tile))
                    {
                        int pathTileId = _tileMap.GetCellv(tile);

                        tileId = pathTileId switch
                        {
                            97 => 50,
                            96 => 51,
                            220 => 50,
                            119 => 51,
                            _ => tileId
                        };
                    }
                    
                    if (_isSnowy)
                    {
                        tileId += 123;
                    }
                    
                    _tileMap.SetCellv(tile, tileId);
                    
                }
            }
        }

        private int GetRiverTileIdFromIncidenceNumber(int incidenceNumber)
        {
            int tileId = incidenceNumber switch
            {
                2 => 57,
                4 => 56,
                6 => 52,
                8 => 58,
                10 => 69,
                12 => 54,
                16 => 59,
                18 => 53,
                20 => 68,
                24 => 55,
                _ => 28
            };

            return tileId;
        }
        
        private int GetPathTileIdFromIncidenceNumber(int incidenceNumber)
        {
            int tileId = incidenceNumber switch
            {
                2 => 43,
                4 => 42,
                6 => 15,
                8 => 44,
                10 => 97,
                12 => 17,
                14 => 24,
                16 => 45,
                18 => 16,
                20 => 96,
                22 => 85,
                24 => 18,
                26 => 87,
                28 => 86,
                30 => 23,
                _ => 28
            };

            return tileId;
        }

        private int CalculateTileIncidencePathNumber(Vector2 tile, HashSet<Vector2> tiles)
        {
            int incidenceNumber = 0;

            if (tiles.Contains(new Vector2(tile.x, tile.y - 1)))
            {
                incidenceNumber += 2;
            }
            
            if (tiles.Contains(new Vector2(tile.x + 1, tile.y)))
            {
                incidenceNumber += 4;
            }
            
            if (tiles.Contains(new Vector2(tile.x, tile.y + 1)))
            {
                incidenceNumber += 8;
            }
            
            if (tiles.Contains(new Vector2(tile.x - 1, tile.y)))
            {
                incidenceNumber += 16;
            }

            return incidenceNumber;
        }
    }
}