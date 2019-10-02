using System;
using Core.Model.Game;
using Core.Model.Space;
using Core.Model.Space.Grid;
using Core.View;

namespace Core
{
    public class Game : IGame
    {
        private readonly int _playerRating;
        private readonly SpaceGrid _spaceGrid;
        private readonly GameConfiguration _conf;
        private readonly ObservablePlanets _planets;
        private readonly SpaceGridTilesVisibilityManager _tilesManager;

        private Square _altView;
        private Square _regView;
        private Position _playerPosition;

        public Game(
            int playerRating,
            SpaceGrid spaceGrid,
            SpaceGridTilesVisibilityManager tilesManager,
            GameConfiguration conf)
        {
            _conf = conf;
            _spaceGrid = spaceGrid;
            _playerRating = playerRating;
            _tilesManager = tilesManager;
            _planets = new ObservablePlanets(_spaceGrid, _conf.AlternativeViewCapacity, _playerRating);
        }

        public State Init(Position playerPosition)
        {
            var offset = _conf.MinZoom / 2;
            var square = new Square(_conf.MinZoom, playerPosition.X - offset, playerPosition.Y - offset);
            _spaceGrid.Traverse(square, _planets.CompositeShow);

            _regView = square;
            _altView = square;
            _playerPosition = playerPosition;

            return CurrentState();
        }

        /// <summary>
        /// Shows + AltShows all the planets on a side, that is next to view squares when moved in given direction.
        /// Hides + AltHides all the planets on a side, that is next to view squares when moved in opposite direction.
        /// </summary>
        public State Move(Direction direction)
        {
            var side = direction.ToSide();
            var delta = direction.ToPositionDelta();
            var offset = Math.Min(delta.X + delta.Y, 0);

            _spaceGrid.Traverse(_regView, side, offset < 0 ? _planets.Show : _planets.Hide, offset);
            _spaceGrid.Traverse(_regView, side, offset < 0 ? _planets.Hide : _planets.Show, offset + _regView.Size);

            _spaceGrid.Traverse(_altView, side, offset < 0 ? _planets.AltShow : _planets.AltHide, offset);
            _spaceGrid.Traverse(_altView, side, offset < 0 ? _planets.AltHide : _planets.AltShow,
                offset + _altView.Size);

            _altView = _altView.Shift(delta);
            _regView = _regView.Shift(delta);
            _playerPosition += delta;

            _tilesManager.OnViewsChanged(_regView, _altView);

            return CurrentState();
        }

        public State Zoom(bool inside)
        {
            if (!CanZoom(inside)) return CurrentState();

            var currentZoom = _altView.Size;

            _altView = ZoomView(ref _altView, inside, _planets.AltShow, _planets.AltHide);

            var affectsRegularView = (inside && currentZoom < _conf.AlternativeViewThreshold) ||
                                     (!inside && currentZoom < _conf.AlternativeViewThreshold - 1);

            if (affectsRegularView)
            {
                _regView = ZoomView(ref _regView, inside, _planets.Show, _planets.Hide);
            }

            _tilesManager.OnViewsChanged(_regView, _altView);

            return CurrentState();
        }

        /// <summary>
        /// Zooms a target view square.
        /// Even zoom values are responsible for top-right outlines of a target view square.
        /// Odd zoom values are responsible for left-bottom outlines of a target view square.
        /// For a selected outline - performs hideAction/showValue based on inside value.
        /// inside == true -> hide
        /// inside == false -> show
        /// </summary>
        private Square ZoomView(
            ref Square view,
            bool inside,
            IPlanetAction showAction,
            IPlanetAction hideAction)
        {
            var targetZoom = view.Size + (inside ? 0 : 1);

            var sideOffset = targetZoom % 2 == 0 ? view.Size : 0;
            var zoomOffset = targetZoom % 2 == 0 ? (inside ? -1 : 0) : (inside ? 0 : -1);
            var totalOffset = sideOffset + zoomOffset;

            var action = inside ? hideAction : showAction;
            var corner = new Position(view.LeftX + totalOffset, view.BottomY + totalOffset);

            var cornerPlanet = _spaceGrid.TryGetPlanet(corner);
            if (cornerPlanet != null) action.Invoke(corner, cornerPlanet.Value);
            _spaceGrid.Traverse(view, Side.Vertical, action, totalOffset);
            _spaceGrid.Traverse(view, Side.Horizontal, action, totalOffset);

            var viewSideOffset = inside ? -1 : 1;
            var viewPositionOffset = targetZoom % 2 != 0 ? (inside ? 1 : -1) : 0;

            return new Square(
                view.Size + viewSideOffset,
                view.LeftX + viewPositionOffset,
                view.BottomY + viewPositionOffset
            );
        }

        private bool CanZoom(bool inside)
        {
            var nextZoom = _altView.Size + (inside ? -1 : 1);

            return _conf.MinZoom <= nextZoom && nextZoom <= _conf.MaxZoom;
        }

        private State CurrentState()
        {
            var isRegularView = _altView.Size < _conf.AlternativeViewThreshold;

            var observablePlanets = isRegularView
                ? _planets.GetObservablePlanets()
                : _planets.GetAltObservablePlanets();

            return new State(
                _altView.Size,
                _playerRating,
                isRegularView,
                _playerPosition,
                observablePlanets
            );
        }
    }
}