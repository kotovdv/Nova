namespace Core.Model.Space.Grid
{
    public class SpaceGridNavigator
    {
        private readonly int _tileSize;
        private readonly int _zeroIndexOffset;

        public SpaceGridNavigator(int tileSize)
        {
            _tileSize = tileSize;
            _zeroIndexOffset = _tileSize / 2;
        }

        /// <summary>
        /// Returns position of a tile and element in given tile where target pos is located.
        /// </summary>
        /// <param name="pos">Target position</param>
        /// <returns>Coordinates of a target position</returns>
        public GridPosition Find(Position pos)
        {
            var tile = FindTile(pos);

            var xPosRemained = pos.X - tile.X * _tileSize;
            var yPosRemained = pos.Y - tile.Y * _tileSize;

            var x = _zeroIndexOffset + xPosRemained;
            var y = _zeroIndexOffset + yPosRemained;

            return new GridPosition(tile, new Position(x, y));
        }

        /// <summary>
        /// Returns position of a tile where target pos is located.
        /// </summary>
        /// <param name="pos">Target position</param>
        /// <returns>Coordinate of a tile with target position</returns>
        public Position FindTile(Position pos)
        {
            var tileX = pos.X >= 0
                ? (pos.X + _zeroIndexOffset) / _tileSize
                : (1 + pos.X + _zeroIndexOffset - _tileSize) / _tileSize;

            var tileY = pos.Y >= 0
                ? (pos.Y + _zeroIndexOffset) / _tileSize
                : (1 + pos.Y + _zeroIndexOffset - _tileSize) / _tileSize;

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