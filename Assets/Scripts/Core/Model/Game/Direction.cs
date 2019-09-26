using System.Collections.Generic;
using Core.Model.Space;

namespace Core.Model.Game
{
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

        private static readonly IDictionary<Direction, Direction> Opposites = new Dictionary<Direction, Direction>
        {
            {Direction.Left, Direction.Right},
            {Direction.Right, Direction.Left},
            {Direction.Up, Direction.Down},
            {Direction.Down, Direction.Up}
        };

        public static Position ToPosition(this Direction direction)
        {
            return Mapping[direction];
        }

        public static Direction ToOpposite(this Direction direction)
        {
            return Opposites[direction];
        }
    }
}