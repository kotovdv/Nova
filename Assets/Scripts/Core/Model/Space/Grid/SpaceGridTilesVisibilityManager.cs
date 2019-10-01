using System.Collections.Generic;

namespace Core.Model.Space.Grid
{
    public class SpaceGridTilesVisibilityManager
    {
        private const int ShowOffset = 1;
        private const int HideOffset = 3;
        private readonly int _tileSize;
        private readonly SpaceGridNavigator _navigator;
        private readonly ISpaceGridTileCache _tilesCache;

        private readonly ISet<Position> RegVisibleTiles = new HashSet<Position>();
        private readonly ISet<Position> AltVisibleTiles = new HashSet<Position>();

        public SpaceGridTilesVisibilityManager(int tileSize, SpaceGridNavigator navigator)
        {
            _tileSize = tileSize;
            _navigator = navigator;
        }

        public void Init(Position startingTile)
        {
            for (var x = -ShowOffset; x <= ShowOffset; x += ShowOffset)
            for (var y = -ShowOffset; y <= ShowOffset; y += ShowOffset)
            {
                var tilePosition = startingTile + new Position(x, y);

                RegVisibleTiles.Add(tilePosition);
                AltVisibleTiles.Add(tilePosition);
                _tilesCache.Load(tilePosition);
            }
        }

        public CacheUpdate OnViewSquaresChanged(Square view)
        {
            var bottomLeftTile = _navigator.FindTile(view.BottomLeft());
            var topLeftTile = _navigator.FindTile(view.TopLeft());
            var bottomRightTile = _navigator.FindTile(view.BottomRight());
            var topRightTile = _navigator.FindTile(view.TopRight());

            ISet<Position> show = new HashSet<Position>();
            ISet<Position> hide = new HashSet<Position>();

            //Vertical left side
            var leftX = bottomLeftTile.X;
            var rightBorder = bottomRightTile.X - ShowOffset;

            for (var x = bottomLeftTile.X; x <= bottomRightTile.X; x++)
            for (var y = bottomLeftTile.Y; y <= topLeftTile.Y; y++)
            {
                var position = new Position(x, y);
                Show(position, show);
            }

            for (var x = bottomLeftTile.X; x <= bottomRightTile.X; x++)
            for (var y = bottomLeftTile.Y; y <= topLeftTile.Y; y++)
            {
                var position = new Position(x, y);
                Hide(position, show, hide);
            }

            return new CacheUpdate(hide, show);
        }


        private static void Show(Position position, ISet<Position> show)
        {
            for (var x = -ShowOffset; x <= ShowOffset; x += ShowOffset)
            for (var y = -ShowOffset; y <= ShowOffset; y += ShowOffset)
            {
                if (x == 0 && y == 0) continue;
                show.Add(position + new Position(x, y));
            }
        }

        private static void Hide(Position position, ICollection<Position> show, ISet<Position> hide)
        {
            for (var x = -HideOffset; x <= HideOffset; x += HideOffset)
            for (var y = -HideOffset; y <= HideOffset; y += HideOffset)
            {
                if (x == 0 && y == 0) continue;

                var hidePosition = position + new Position(x, y);
                if (!show.Contains(hidePosition))
                {
                    hide.Add(hidePosition);
                }
            }
        }

        public struct CacheUpdate
        {
            private ISet<Position> _hide;
            private ISet<Position> _show;

            public CacheUpdate(ISet<Position> hide, ISet<Position> show)
            {
                this._hide = hide;
                this._show = show;
            }
        }
    }
}