using Godot;


namespace TowerDefenseMC.Utils
{
    public static class Vector2Helper
    {
        public static Vector2 CartesianToIsometric(this Vector2 vec)
        {
            return new Vector2((vec.x - vec.y) * 64, ((vec.x + vec.y) / 2) * 64);
        }
    }
}