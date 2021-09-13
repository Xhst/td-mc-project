using Godot;

[Tool]
public class TileMap : Godot.TileMap
{

    [Export] private Vector2 _offset;

    public override void _Ready()
    {
        foreach(int tile in TileSet.GetTilesIds())
        {
            TileSet.TileSetTextureOffset(tile, _offset);
        }
    }
}
