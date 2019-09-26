using System;

namespace Core.Model.Space
{
    public class SpaceGridNavigator
    {
        private readonly int _tileSize;

        public SpaceGridNavigator(int tileSize)
        {
            _tileSize = tileSize;
        }

        public TargetPosition Find(Position position)
        {
            var xPos = position.X;
            var yPos = position.Y;

            var xOffset = position.Y < 0 ? 1 : 0;
            var yOffset = position.X < 0 ? 1 : 0;

            var gridX = (xPos >= 0 ? 1 : -1) + ((xPos + yOffset) / _tileSize);
            var gridY = (yPos >= 0 ? 1 : -1) + ((yPos + xOffset) / _tileSize);

            var xPosRemained = Math.Abs((xPos + xOffset) % _tileSize);
            var yPosRemained = Math.Abs((yPos + yOffset) % _tileSize);

            var tileRow = xPos >= 0 ? xPosRemained : (_tileSize - 1) - xPosRemained;
            var tileColumn = yPos >= 0 ? yPosRemained : (_tileSize - 1) - yPosRemained;

            return new TargetPosition(
                new Position(gridX, gridY),
                new Position(tileRow, tileColumn)
            );
        }
    }

    public readonly struct TargetPosition
    {
        public readonly Position GridPosition;
        public readonly Position TilePosition;

        public TargetPosition(Position gridPosition, Position tilePosition)
        {
            GridPosition = gridPosition;
            TilePosition = tilePosition;
        }
    }
}