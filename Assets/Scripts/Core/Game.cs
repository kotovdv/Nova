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
        private readonly ObservablePlanets _observablePlanets;
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
            _observablePlanets = new ObservablePlanets(_spaceGrid, _conf.AlternativeViewCapacity, _playerRating);
        }

        public State Init()
        {
            _zoom = _conf.MinZoom;
            var offset = _zoom / 2;

            _leftX = _playerPosition.X - offset;
            _bottomY = _playerPosition.Y - offset;
            _altLeftX = _leftX;
            _altBottomY = _bottomY;

            _spaceGrid.Traverse(_leftX, _leftX + _zoom, _bottomY, _bottomY + _zoom, _observablePlanets.CombinedShow);

            return CurrentState();
        }

        public State Move(Direction direction)
        {
            var delta = direction.ToPosition();
            _playerPosition += delta;

            var altViewSize = _zoom;
            _spaceGrid.Traverse(_altLeftX, _altBottomY, altViewSize, direction, _observablePlanets.AltShow);
            _spaceGrid.Traverse(_altLeftX + delta.X, _altBottomY + delta.Y, altViewSize, direction.ToOpposite(),
                _observablePlanets.AltHide);
            _altLeftX += delta.X;
            _altBottomY += delta.Y;

            var regularViewSize = Math.Min(_conf.AlternativeViewThreshold - 1, _zoom);
            _spaceGrid.Traverse(_leftX, _bottomY, regularViewSize, direction, _observablePlanets.Show);
            _spaceGrid.Traverse(_leftX + delta.X, _bottomY + delta.Y, regularViewSize, direction.ToOpposite(),
                (position, planet) => _observablePlanets.Hide(position));
            _leftX += delta.X;
            _bottomY += delta.Y;

            return CurrentState();
        }

        public State Zoom(bool inside)
        {
            if (!CanZoom(inside)) return CurrentState();

            _zoom += inside ? -1 : 1;
            var isAltView = _zoom >= _conf.AlternativeViewThreshold;

            var action = inside
                ? (isAltView ? (Action<Position, Planet>) _observablePlanets.AltHide : _observablePlanets.CombinedHide)
                : (isAltView ? (Action<Position, Planet>) _observablePlanets.AltShow : _observablePlanets.CombinedShow);

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
                ? _observablePlanets.GetObservablePlanets()
                : _observablePlanets.GetAltObservablePlanets();

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