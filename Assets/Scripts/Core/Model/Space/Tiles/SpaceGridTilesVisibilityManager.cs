using System;
using System.Collections.Generic;
using Core.Model.Space.Tiles;

namespace Core.Model.Space.Grid
{
    public class SpaceGridTilesVisibilityManager
    {
        private const int ShowOffset = 1;
        private const int HideOffset = 3;

        private readonly int _tileSize;
        private readonly SpaceGridNavigator _navigator;
        private readonly SpaceGridTileCache _tilesCache;

        private readonly IList<Action<Position>> _onShouldBeHiddenCallback = new List<Action<Position>>();
        private readonly IList<Action<Position>> _onShouldBecomeVisibleCallback = new List<Action<Position>>();

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

        public void SubscribeOnShowTile(Action<Position> callback)
        {
            _onShouldBecomeVisibleCallback.Add(callback);
        }

        public void SubscribeOnHideTile(Action<Position> callback)
        {
            _onShouldBeHiddenCallback.Add(callback);
        }

        public void OnViewsChanged(Square regView, Square altView)
        {
            if (altView.BottomY % _tileSize == 0 || altView.LeftX % _tileSize == 0)
            {
                var modifier = (HideOffset * 4 + 1) * _tileSize;
                var outerBounds = new Square(
                    altView.Size + 2 * modifier,
                    altView.LeftX - modifier,
                    altView.BottomY - modifier
                );
                OnViewChanged(altView, outerBounds, regView);
            }

            if (regView.Equals(altView)) return;

            if (regView.BottomY % _tileSize == 0 || regView.LeftX % _tileSize == 0)
            {
                OnViewChanged(regView, altView, regView);
            }
        }

        private void OnViewChanged(Square view, Square outerBounds, Square innerBounds)
        {
            var innerTopRightTile = _navigator.FindTile(innerBounds.TopRight());
            var innerBottomLeftTile = _navigator.FindTile(innerBounds.BottomLeft());

            var outerTopRightTile = _navigator.FindTile(outerBounds.TopRight());
            var outerBottomLeftTile = _navigator.FindTile(outerBounds.BottomLeft());

            var viewTopRightTile = _navigator.FindTile(view.TopRight());
            var viewBottomLeftTile = _navigator.FindTile(view.BottomLeft());

            for (var a = 0; a < 2; a++)
            {
                var axis = (Axis) a;
                var oppositeAxis = axis.Opposite();
                for (var side = 0; side < 2; side++)
                {
                    var showOffsetPos = new Position(axis == Axis.X ? ShowOffset : 0, axis == Axis.Y ? ShowOffset : 0);
                    var hideOffsetPos = new Position(axis == Axis.X ? HideOffset : 0, axis == Axis.Y ? HideOffset : 0);

                    for (var point = viewBottomLeftTile.At(oppositeAxis);
                        point <= viewTopRightTile.At(oppositeAxis);
                        point++)
                    {
                        var tile = axis == Axis.X
                            ? new Position(side == 0 ? viewBottomLeftTile.X : viewTopRightTile.X, point)
                            : new Position(point, side == 0 ? viewBottomLeftTile.Y : viewTopRightTile.Y);

                        if (side == 0)
                        {
                            var showTile = tile - showOffsetPos;
                            if (showTile.At(axis) > (outerBottomLeftTile.At(axis) + showOffsetPos.At(axis)))
                                Show(showTile);

                            var hideTile = tile - hideOffsetPos;
                            if (hideTile.At(axis) > (outerBottomLeftTile.At(axis) + hideOffsetPos.At(axis)))
                                Hide(hideTile);
                        }
                        else
                        {
                            var showTile = tile + showOffsetPos;
                            if (showTile.At(axis) < (outerTopRightTile.At(axis) - showOffsetPos.At(axis)))
                                Show(showTile);

                            var hideTile = tile + hideOffsetPos;
                            if (hideTile.At(axis) < (outerTopRightTile.At(axis) - hideOffsetPos.At(axis)))
                                Hide(hideTile);
                        }

                        if (point == viewBottomLeftTile.At(oppositeAxis) || point == viewTopRightTile.At(oppositeAxis))
                            continue;

                        if (side == 0)
                        {
                            var showTile = tile + showOffsetPos;
                            if (showTile.At(axis) < (innerBottomLeftTile.At(axis) - showOffsetPos.At(axis)))
                                Show(showTile);

                            var hideTile = tile + hideOffsetPos;
                            if (hideTile.At(axis) < (innerBottomLeftTile.At(axis) - hideOffsetPos.At(axis)))
                                Hide(hideTile);
                        }
                        else
                        {
                            var showTile = tile - showOffsetPos;
                            if (showTile.At(axis) > (innerTopRightTile.At(axis) + showOffsetPos.At(axis)))
                                Show(showTile);

                            var hideTile = tile - hideOffsetPos;
                            if (hideTile.At(axis) > (innerTopRightTile.At(axis) + hideOffsetPos.At(axis)))
                                Hide(hideTile);
                        }
                    }
                }
            }
        }

        private void Show(Position position)
        {
            _tilesCache.LoadAsync(position);
        }

        private void Hide(Position position)
        {
            _tilesCache.UnloadAsync(position);
        }
    }
}