using System;
using System.Collections.Generic;
using Core.Configuration;
using Core.Model.Game;
using Core.Util;

namespace Core.Model.Space
{
    public class SpaceGrid
    {
        private readonly int _tileSize;
        private readonly SpaceTileFactory _tileFactory;
        private readonly SpaceGridNavigator _navigator;
        private readonly IDictionary<Position, SpaceTile> _grid = new Dictionary<Position, SpaceTile>();

        public SpaceGrid(int tileSize, SpaceTileFactory tileFactory)
        {
            _tileSize = tileSize;
            _tileFactory = tileFactory;
            _navigator = new SpaceGridNavigator(tileSize);
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
            var targetPosition = _navigator.Find(position);
            var gridPosition = targetPosition.GridPosition;
            var tilePos = targetPosition.TilePosition;

            var tile = _grid.GetOrCompute(gridPosition, () => _tileFactory.CreateTile());

            return tile[tilePos.X, tilePos.Y];
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