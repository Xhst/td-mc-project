﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Godot;

using TowerDefenseMC.Singletons;
using TowerDefenseMC.Utils;


namespace TowerDefenseMC.Levels
{
    public class TerrainBuilder
    {
        private const int FirstSnowTileId = 123;
        private const float TimeBetweenTileDraw = 0.01f;
        
        private const int TileLength = 128;
        private const int TileHeight = 64;

        private readonly LevelTemplate _levelTemplate;

        private readonly TileMap _tileMap;
        private readonly bool _isSnowy;
        
        private readonly int _fillTileId;

        public TerrainBuilder(LevelTemplate levelTemplate, bool isSnowy, TileMap tileMap)
        {
            _levelTemplate = levelTemplate;
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
                    
                    SetCell(newTilePos, _fillTileId);

                    if (x < length)
                    {
                        SetCell((int) newTilePos.x + 1, (int) newTilePos.y, _fillTileId);
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
        
        public async Task DrawCustomTiles(Dictionary<string, List<TilePosition>> tiles)
        {
            foreach (KeyValuePair<string, List<TilePosition>> tile in tiles)
            {
                foreach (TilePosition tilePosition in tile.Value)
                {
                    string tileName = _isSnowy ? "snow_" + tile.Key : tile.Key;
                    
                    int tileId = FindTileByNameTryingRotation(tileName, tilePosition.Rot);
                    
                    SetCell(tilePosition.X, tilePosition.Y, tileId);
                    
                    await _levelTemplate.ToSignal(_levelTemplate.GetTree().CreateTimer(TimeBetweenTileDraw), "timeout");
                }
            }
        }

        public async Task DrawPathsAndRivers(List<List<Vector2>> paths, List<List<Vector2>> rivers)
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
            
            await DrawPaths(paths, pathTiles);
            await DrawRivers(rivers, riverTiles, pathTiles);
        }
        
        private async Task DrawPaths(List<List<Vector2>> paths, HashSet<Vector2> pathTiles)
        {
            foreach (List<Vector2> path in paths)
            {
                foreach (Vector2 tile in path)
                {
                    int incidenceNumber = CalculateTileIncidencePathNumber(tile, pathTiles);
                    int tileId = GetPathTileIdFromIncidenceNumber(incidenceNumber);

                    if (tile == path.First())
                    {
                        tileId = GetStartPathTileId(incidenceNumber);
                    }

                    if (tile == path.Last())
                    {
                        tileId = GetLastPathTileId(incidenceNumber);
                    }

                    DrawTile(tile, tileId);
                    
                    await _levelTemplate.ToSignal(_levelTemplate.GetTree().CreateTimer(TimeBetweenTileDraw), "timeout");
                }
            }
        }
        
        private async Task DrawRivers(List<List<Vector2>> rivers, HashSet<Vector2> riverTiles, HashSet<Vector2> pathTiles)
        {
            foreach (List<Vector2> river in rivers)
            {
                foreach (Vector2 tile in river)
                {
                    int incidenceNumber = CalculateTileIncidencePathNumber(tile, riverTiles);
                    int tileId = GetRiverTileIdFromIncidenceNumber(incidenceNumber);

                    if (pathTiles.Contains(tile))
                    {
                        tileId = GetBridgeTileId(tileId, tile);
                    }
                    
                    DrawTile(tile, tileId);
                    await _levelTemplate.ToSignal(_levelTemplate.GetTree().CreateTimer(TimeBetweenTileDraw), "timeout");
                }
            }
        }

        private void DrawTile(Vector2 tile, int tileId)
        {
            if (_isSnowy)
            {
                tileId = GetTileIdSnowyVersion(tileId);
            }
            
            SetCell(tile, tileId);
            
        }

        private void SetCell(Vector2 position, int tileId)
        {
            SetCell((int) position.x, (int) position.y, tileId);
        }
        
        private void SetCell(int x, int y, int tileId)
        {
            tileId %= FirstSnowTileId * 2;
            
            _tileMap.SetCell(x, y, tileId);
        }

        private int GetStartPathTileId(int incidenceNumber)
        {
            return incidenceNumber switch
            {
                2 => 31,
                4 => 30,
                8 => 32,
                16 => 33,
                _ => 30
            };
        }
        private int GetLastPathTileId(int incidenceNumber)
        {
            return incidenceNumber switch
            {
                2 => 39,
                4 => 38,
                8 => 40,
                16 => 41,
                _ => 38
            };
        }

        private int GetTileIdSnowyVersion(int tileId)
        {
            return tileId + FirstSnowTileId;
        }

        private int GetBridgeTileId(int currentTileId, Vector2 tile)
        {
            int pathTileId = _tileMap.GetCellv(tile);

            return pathTileId switch
            {
                97 => 50,
                96 => 51,
                220 => 50,
                119 => 51,
                _ => currentTileId
            };
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

            // Top
            if (tiles.Contains(new Vector2(tile.x, tile.y - 1)))
            {
                incidenceNumber += 2;
            }
            
            // Right
            if (tiles.Contains(new Vector2(tile.x + 1, tile.y)))
            {
                incidenceNumber += 4;
            }
            
            // Bottom
            if (tiles.Contains(new Vector2(tile.x, tile.y + 1)))
            {
                incidenceNumber += 8;
            }
            
            // Left
            if (tiles.Contains(new Vector2(tile.x - 1, tile.y)))
            {
                incidenceNumber += 16;
            }

            return incidenceNumber;
        }
    }
}