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

            _spaceGrid.Traverse(_leftX, _leftX + _zoom, _bottomY, _bottomY + _zoom, _planets.CombinedShow);

            return CurrentState();
        }

        public State Move(Direction direction)
        {
            var altView = new Square(_zoom, _altLeftX, _altBottomY);
            var regView = new Square(Math.Min(_conf.AlternativeViewThreshold - 1, _zoom), _leftX, _bottomY);

            var side = direction.ToSide();
            var posDelta = direction.ToPositionDelta();
            var offset = Math.Min(posDelta.X + posDelta.Y, 0);

            if (direction == Direction.Left || direction == Direction.Down)
            {
                _spaceGrid.Traverse(regView, side, _planets.Show, offset);
                _spaceGrid.Traverse(regView, side, _planets.Hide, offset + regView.Size);

                _spaceGrid.Traverse(altView, side, _planets.AltShow, offset);
                _spaceGrid.Traverse(altView, side, _planets.AltHide, offset + altView.Size);
            }
            else
            {
                _spaceGrid.Traverse(regView, side, _planets.Hide, offset);
                _spaceGrid.Traverse(regView, side, _planets.Show, offset + regView.Size);

                _spaceGrid.Traverse(altView, side, _planets.AltHide, offset);
                _spaceGrid.Traverse(altView, side, _planets.AltShow, offset + altView.Size);
            }


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

            _zoom += inside ? -1 : 1;
            var isAltView = _zoom >= _conf.AlternativeViewThreshold;

            var action = inside
                ? (isAltView ? (Action<Position, Planet>) _planets.AltHide : _planets.CombinedHide)
                : (isAltView ? (Action<Position, Planet>) _planets.AltShow : _planets.CombinedShow);

            if ((_zoom + (inside ? 1 : 0)) % 2 == 0)
            {
                var zoomOffset = !inside ? -1 : 0;

                _spaceGrid.TraverseTopToRight(
                    isAltView ? _altLeftX : _leftX,
                    isAltView ? _altBottomY : _bottomY,
                    _zoom + zoomOffset,
                    action
                );
            }
            else
            {
                var offset = (inside ? 1 : 0);
                var zoomOffset = !inside ? -1 : 0;

                _spaceGrid.TraverseBottomToLeft(
                    (isAltView ? _altLeftX : _leftX) + offset,
                    (isAltView ? _altBottomY : _bottomY) + offset,
                    _zoom + zoomOffset,
                    action
                );

                var delta = inside ? 1 : -1;

                _altLeftX += delta;
                _altBottomY += delta;
                if (IsRegularView)
                {
                    _leftX += delta;
                    _bottomY += delta;
                }
            }


            return CurrentState();
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