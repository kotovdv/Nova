using System;
using Core.Model.Game;
using Core.Model.Space;
using Core.View;

namespace Core
{
    public class Game : IGame
    {
        private readonly int _playerRating;
        private readonly SpaceGrid _spaceGrid;
        private readonly GameConfiguration _conf;

        private int _zoom;
        private int _leftX, _altLeftX;
        private int _bottomY, _altBottomY;
        private Position _playerPosition;
        private readonly ObservablePlanets _planets;
        private bool IsRegularView => _zoom < _conf.AlternativeViewThreshold;

        public Game(
            int playerRating,
            Position playerPosition,
            SpaceGrid spaceGrid,
            GameConfiguration conf)
        {
            _conf = conf;
            _spaceGrid = spaceGrid;
            _playerRating = playerRating;
            _playerPosition = playerPosition;
            _planets = new ObservablePlanets(_spaceGrid, _conf.AlternativeViewCapacity, _playerRating);
        }

        public State Init()
        {
            _zoom = _conf.MinZoom;
            var offset = _zoom / 2;

            _leftX = _playerPosition.X - offset;
            _bottomY = _playerPosition.Y - offset;
            _altLeftX = _leftX;
            _altBottomY = _bottomY;

            _spaceGrid.Traverse(_leftX, _leftX + _zoom, _bottomY, _bottomY + _zoom, _planets.CompositeShow);

            return CurrentState();
        }

        public State Move(Direction direction)
        {
            var altView = new Square(_zoom, _altLeftX, _altBottomY);
            var regView = new Square(Math.Min(_conf.AlternativeViewThreshold - 1, _zoom), _leftX, _bottomY);

            var side = direction.ToSide();
            var posDelta = direction.ToPositionDelta();
            var offset = Math.Min(posDelta.X + posDelta.Y, 0);

            _spaceGrid.Traverse(regView, side, offset < 0 ? _planets.Show : _planets.Hide, offset);
            _spaceGrid.Traverse(regView, side, offset < 0 ? _planets.Hide : _planets.Show, offset + regView.Size);

            _spaceGrid.Traverse(altView, side, offset < 0 ? _planets.AltShow : _planets.AltHide, offset);
            _spaceGrid.Traverse(altView, side, offset < 0 ? _planets.AltHide : _planets.AltShow, offset + altView.Size);

            _leftX += posDelta.X;
            _bottomY += posDelta.Y;
            _altLeftX += posDelta.X;
            _altBottomY += posDelta.Y;
            _playerPosition += posDelta;

            return CurrentState();
        }

        public State Zoom(bool inside)
        {
            if (!CanZoom(inside)) return CurrentState();

            var altView = new Square(_zoom, _altLeftX, _altBottomY);

            var altResult = ZoomView(_zoom, inside, ref altView, _planets.AltShow, _planets.AltHide);
            _altLeftX = altResult.LeftX;
            _altBottomY = altResult.BottomY;

            var targetZoom = _zoom + (inside ? -1 : 1);
            if (!inside && _zoom < _conf.AlternativeViewThreshold - 1 ||
                inside && _zoom < _conf.AlternativeViewThreshold)
            {
                var regView = new Square(Math.Min(_conf.AlternativeViewThreshold - 1, _zoom), _leftX, _bottomY);

                var result = ZoomView(_zoom, inside, ref regView, _planets.Show, _planets.Hide);
                _leftX = result.LeftX;
                _bottomY = result.BottomY;
            }

            _zoom = targetZoom;

            return CurrentState();
        }

        //Even sides are top and right
        //Odd sides are bottom and left
        private Square ZoomView(
            int zoom,
            bool inside,
            ref Square view,
            IPlanetAction showAction,
            IPlanetAction hideAction)
        {
            var targetZoom = zoom + (inside ? 0 : 1);

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
            var delta = inside ? -1 : 1;

            return _conf.MinZoom <= (_zoom + delta) && (_zoom + delta) <= _conf.MaxZoom;
        }

        private State CurrentState()
        {
            var observablePlanets = IsRegularView
                ? _planets.GetObservablePlanets()
                : _planets.GetAltObservablePlanets();

            return new State(
                _zoom,
                _playerRating,
                IsRegularView,
                _playerPosition,
                observablePlanets
            );
        }
    }
}