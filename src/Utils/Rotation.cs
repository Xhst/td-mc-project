namespace TowerDefenseMC.Utils
{
    public enum Rotation
    {
        North,
        East,
        South,
        West
    }

    public static class RotationUtils
    {
        public static Rotation StringToDirection(string rotation)
        {
            return rotation?.ToUpper() switch
            {
                "N" => Rotation.North,
                "E" => Rotation.East,
                "S" => Rotation.South,
                "W" => Rotation.West,
                _ => Rotation.North
            };
        }

        public static string ToStringShort(this Rotation rotation)
        {
            return rotation switch
            {
                Rotation.North => "N",
                Rotation.East => "E",
                Rotation.South => "S",
                Rotation.West => "W",
                _ => "N"
            };
        }
        
        public static string ToStringPair(this Rotation rotation)
        {
            return rotation switch
            {
                Rotation.North => "NS",
                Rotation.South => "NS",
                Rotation.East => "EW",
                Rotation.West => "EW",
                _ => "NS"
            };
        }
    }
}