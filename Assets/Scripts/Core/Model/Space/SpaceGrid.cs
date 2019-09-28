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
        private readonly ISpaceTileProvider _tileProvider;
        private readonly SpaceGridNavigator _navigator;
        private readonly IDictionary<Position, SpaceTile> _grid = new Dictionary<Position, SpaceTile>();

        public SpaceGrid(int tileSize, ISpaceTileProvider tileProvider)
        {
            _tileSize = tileSize;
            _tileProvider = tileProvider;
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

        public Planet? TryGetPlanet(Position position)
        {
            var targetPosition = _navigator.Find(position);
            var gridPosition = targetPosition.GridPosition;
            var tilePos = targetPosition.TilePosition;

            var tile = _grid.GetOrCompute(gridPosition, () => _tileProvider.Take());

            return tile[tilePos.X, tilePos.Y];
        }

        /// <summary>
        /// Executes given action for each element at target location.
        /// Target location is determined via combination of square, side and offset.
        /// For example
        /// Square = (leftX = 0, bottomY = 0, size = 10)
        /// Side = Vertical
        /// Offset = 0
        /// Will result in
        /// Traversing all elements from
        /// (0,0)
        /// (0,1)
        /// ...
        /// (0,9)
        /// Changing offset to 1 will result in
        /// (1,0)
        /// (1,1)
        /// ...
        /// (1,9)
        /// being traversed.
        /// </summary>
        /// <param name="square">Information about a square from the grid.</param>
        /// <param name="side">Which side of a square is being traversed (vertical or horizontal).</param>
        /// <param name="action">Action that will be performed on all elements during traversal.</param>
        /// <param name="offset">Offset from leftX, bottomY in X for vertical side and Y for horizontal side.</param>
        public void Traverse(Square square, Side side, IPlanetAction action, int offset = 0)
        {
            var initialX = square.LeftX + (side == Side.Vertical ? offset : 0);
            var initialY = square.BottomY + (side == Side.Horizontal ? offset : 0);

            var finalX = (side == Side.Horizontal) ? initialX + square.Size : initialX + 1;
            var finalY = (side == Side.Vertical) ? initialY + square.Size : initialY + 1;

            for (var x = initialX; x < finalX; x++)
            for (var y = initialY; y < finalY; y++)
            {
                var position = new Position(x, y);

                var optPlanet = TryGetPlanet(position);
                if (!optPlanet.HasValue) continue;

                action.Invoke(position, optPlanet.Value);
            }
        }

        public void Traverse(int fromX, int toX, int fromY, int toY, IPlanetAction action)
        {
            for (var x = fromX; x < toX; x++)
            for (var y = fromY; y < toY; y++)
            {
                var position = new Position(x, y);

                var optPlanet = TryGetPlanet(position);
                if (!optPlanet.HasValue) continue;

                action.Invoke(position, optPlanet.Value);
            }
        }
    }
}