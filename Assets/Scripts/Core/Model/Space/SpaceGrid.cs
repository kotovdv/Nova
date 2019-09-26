using System;
using System.Collections.Generic;
using Core.Configuration;
using Core.Model.Game;
using Util;

namespace Core.Model.Space
{
    public class SpaceGrid
    {
        private readonly int _tileSize;
        private readonly SpaceTileFactory _tileFactory;
        private readonly IDictionary<Position, SpaceTile> _grid = new Dictionary<Position, SpaceTile>();

        public SpaceGrid(int tileSize, SpaceTileFactory tileFactory)
        {
            _tileSize = tileSize;
            _tileFactory = tileFactory;
        }

        public Planet GetPlanet(Position position)
        {
            var optional = TryGetPlanet(position);
            if (!optional.HasValue)
            {
                throw new NullReferenceException("No planet at [" + position + "]");
            }

            return optional.Value;
        }

        private Planet? TryGetPlanet(Position position)
        {
            var xPos = position.X;
            var yPos = position.Y;

            var gridRow = (xPos >= 0 ? 1 : -1) + xPos / _tileSize;
            var gridColumn = (yPos >= 0 ? 1 : -1) + yPos / _tileSize;

            var tile = _grid.GetOrCompute(new Position(gridRow, gridColumn), _tileFactory.CreateTile);

            var xPosRemained = Math.Abs(xPos % _tileSize);
            var yPosRemained = Math.Abs(yPos % _tileSize);

            var tileRow = xPos >= 0 ? xPosRemained : _tileSize - xPosRemained;
            var tileColumn = yPos >= 0 ? yPosRemained : _tileSize - yPosRemained;

            return tile[tileRow, tileColumn];
        }

        public void Traverse(int leftX, int bottomY, int size, Direction direction, Action<Position, Planet> action)
        {
            Traverse(leftX, bottomY, size, size, direction, action);
        }

        public void TraverseBottomToLeft(int leftX, int bottomY, int length, Action<Position, Planet> action)
        {
            Traverse(leftX, bottomY, length, length, Direction.Left, action);
            Traverse(leftX - 1, bottomY, length + 1, length, Direction.Down, action);
        }

        public void TraverseTopToRight(int leftX, int bottomY, int length, Action<Position, Planet> action)
        {
            Traverse(leftX, bottomY, length + 1, length, Direction.Up, action);
            Traverse(leftX, bottomY, length, Direction.Right, action);
        }

        public void Traverse(int leftX, int bottomY, int sizeX, int sizeY, Direction direction,
            Action<Position, Planet> action)
        {
            switch (direction)
            {
                case Direction.Left:
                    TraverseVertical(bottomY, bottomY + sizeY, leftX - 1, action);
                    break;
                case Direction.Right:
                    TraverseVertical(bottomY, bottomY + sizeY, leftX + sizeX, action);
                    break;
                case Direction.Up:
                    TraverseHorizontal(leftX, leftX + sizeX, bottomY + sizeY, action);
                    break;
                case Direction.Down:
                    TraverseHorizontal(leftX, leftX + sizeX, bottomY - 1, action);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void Traverse(int fromX, int toX, int fromY, int toY, Action<Position, Planet> action)
        {
            for (var x = fromX; x < toX; x++)
            {
                for (var y = fromY; y < toY; y++)
                {
                    var position = new Position(x, y);

                    var optPlanet = TryGetPlanet(position);
                    if (!optPlanet.HasValue) continue;

                    action.Invoke(position, optPlanet.Value);
                }
            }
        }

        private void TraverseVertical(int fromY, int toY, int x, Action<Position, Planet> action)
        {
            Traverse(x, x + 1, fromY, toY, action);
        }

        private void TraverseHorizontal(int fromX, int toX, int y, Action<Position, Planet> action)
        {
            Traverse(fromX, toX, y, y + 1, action);
        }
    }
}