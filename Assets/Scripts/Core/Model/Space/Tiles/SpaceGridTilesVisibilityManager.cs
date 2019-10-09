using Core.Model.Space.Grid;

namespace Core.Model.Space.Tiles
{
    public class SpaceGridTilesVisibilityManager
    {
        private const int ShowOffset = 1;
        private const int HideOffset = 3;

        private readonly int _tileSize;
        private readonly SpaceGridNavigator _navigator;
        private readonly SpaceGridTileCache _tilesCache;

        public SpaceGridTilesVisibilityManager(
            int tileSize,
            SpaceGridNavigator navigator,
            SpaceGridTileCache tilesCache)
        {
            _tileSize = tileSize;
            _navigator = navigator;
            _tilesCache = tilesCache;
        }

        public void Init(Position startingTile)
        {
            for (var x = -ShowOffset; x <= ShowOffset; x += ShowOffset)
            for (var y = -ShowOffset; y <= ShowOffset; y += ShowOffset)
            {
                var tilePosition = startingTile + new Position(x, y);
                _tilesCache.Load(tilePosition);
            }
        }

        public void OnViewChanged(Square view)
        {
            if (view.BottomY % _tileSize != 0 && view.LeftX % _tileSize != 0) return;

            var topRightTile = _navigator.FindTile(view.TopRight());
            var bottomLeftTile = _navigator.FindTile(view.BottomLeft());

            for (var a = 0; a < 2; a++)
            {
                var axis = (Axis) a;

                var showOffsetPos = new Position(axis == Axis.X ? ShowOffset : 0, axis == Axis.Y ? ShowOffset : 0);
                var hideOffsetPos = new Position(axis == Axis.X ? HideOffset : 0, axis == Axis.Y ? HideOffset : 0);

                for (var side = 0; side < 2; side++)
                {
                    var oppositeAxis = axis.Opposite();

                    for (var p = bottomLeftTile.At(oppositeAxis); p <= topRightTile.At(oppositeAxis); p++)
                    {
                        Position tile;

                        //Left and Right sides
                        if (axis == Axis.X)
                        {
                            tile = side == 0 ? new Position(bottomLeftTile.X, p) : new Position(topRightTile.X, p);
                        }
                        //Bottom and Top sides
                        else
                        {
                            tile = side == 0 ? new Position(p, bottomLeftTile.Y) : new Position(p, topRightTile.Y);
                        }

                        _tilesCache.LoadAsync(tile + (side == 0 ? -showOffsetPos : showOffsetPos));
                        _tilesCache.UnloadAsync(tile + (side == 0 ? -hideOffsetPos : hideOffsetPos));
                    }
                }
            }
            
            //Bottom left
            _tilesCache.LoadAsync(new Position(bottomLeftTile.X - ShowOffset, bottomLeftTile.Y - ShowOffset));
            _tilesCache.LoadAsync(new Position(bottomLeftTile.X - HideOffset, bottomLeftTile.Y - HideOffset));

            //Bottom right
            _tilesCache.LoadAsync(new Position(topRightTile.X + ShowOffset, bottomLeftTile.Y - ShowOffset));
            _tilesCache.LoadAsync(new Position(topRightTile.X + HideOffset, bottomLeftTile.Y - HideOffset));
            
            //Top left
            _tilesCache.LoadAsync(new Position(bottomLeftTile.X - ShowOffset, topRightTile.Y + ShowOffset));
            _tilesCache.LoadAsync(new Position(bottomLeftTile.X - HideOffset, topRightTile.Y + HideOffset));

            //Top right
            _tilesCache.LoadAsync(new Position(topRightTile.X + ShowOffset, topRightTile.Y + ShowOffset));
            _tilesCache.LoadAsync(new Position(topRightTile.X + HideOffset, topRightTile.Y + HideOffset));
        }
    }
}