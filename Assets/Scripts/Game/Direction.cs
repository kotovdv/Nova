using System.Collections.Generic;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

public static class DirectionExtensions
{
    private static readonly IDictionary<Direction, Position> Mapping = new Dictionary<Direction, Position>
    {
        {Direction.Left, new Position(-1, 0)},
        {Direction.Right, new Position(1, 0)},
        {Direction.Up, new Position(0, 1)},
        {Direction.Down, new Position(0, -1)}
    };

    public static Position ToPosition(this Direction direction)
    {
        return Mapping[direction];
    }
}