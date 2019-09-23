using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right,
}

public static class DirectionExtensions
{
    private static readonly IDictionary<Direction, Vector2> Mapping = new Dictionary<Direction, Vector2>
    {
        {Direction.None, Vector2.zero},
        {Direction.Left, Vector2.left},
        {Direction.Right, Vector2.right},
        {Direction.Up, Vector2.up},
        {Direction.Down, Vector2.down}
    };

    public static Vector2 ToVector2(this Direction direction)
    {
        return Mapping[direction];
    }
}