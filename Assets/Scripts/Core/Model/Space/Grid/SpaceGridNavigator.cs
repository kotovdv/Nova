using System;

namespace Core.Model.Space.Grid
{
    public class SpaceGridNavigator
    {
        private readonly int _tileSize;

        public SpaceGridNavigator(int tileSize)
        {
            _tileSize = tileSize;
        }

        public GridPosition Find(Position position)
        {
            var xPos = position.X;
            var yPos = position.Y;

            var xOffset = position.Y < 0 ? 1 : 0;
            var yOffset = position.X < 0 ? 1 : 0;

            var tileX = (xPos >= 0 ? 1 : -1) + ((xPos + yOffset) / _tileSize);
            var tileY = (yPos >= 0 ? 1 : -1) + ((yPos + xOffset) / _tileSize);

            var xPosRemained = Math.Abs((xPos + xOffset) % _tileSize);
            var yPosRemained = Math.Abs((yPos + yOffset) % _tileSize);

            var elemRow = xPos >= 0 ? xPosRemained : (_tileSize - 1) - xPosRemained;
            var elemColumn = yPos >= 0 ? yPosRemained : (_tileSize - 1) - yPosRemained;

            return new GridPosition(
                new Position(tileX, tileY),
                new Position(elemRow, elemColumn)
            );
        }

        //TODO rework
        public Position FindTile(Position position)
        {
            var xPos = position.X;
            var yPos = position.Y;

            var xOffset = position.Y < 0 ? 1 : 0;
            var yOffset = position.X < 0 ? 1 : 0;

            var tileX = (xPos >= 0 ? 1 : -1) + ((xPos + yOffset) / _tileSize);
            var tileY = (yPos >= 0 ? 1 : -1) + ((yPos + xOffset) / _tileSize);

            return new Position(tileX, tileY);
        }
    }

    public readonly struct GridPosition
    {
        public readonly Position TilePosition;
        public readonly Position ElementPosition;

        public GridPosition(Position tilePosition, Position elementPosition)
        {
            TilePosition = tilePosition;
            ElementPosition = elementPosition;
        }
    }
}