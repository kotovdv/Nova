using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public partial class Game : IGame
{
    private readonly int _playerRating;
    private readonly Configuration _conf;
    private readonly SpaceGrid _spaceGrid;

    private int _zoom;
    private int _leftX, _altLeftX;
    private int _bottomY, _altBottomY;
    private Position _playerPosition;

    public Game(
        int playerRating,
        Position playerPosition,
        SpaceGrid spaceGrid,
        Configuration conf)
    {
        _playerRating = playerRating;
        _playerPosition = playerPosition;
        _spaceGrid = spaceGrid;
        _conf = conf;
        _alternativeViewSet = new AlternativeViewSet(conf.AlternativeViewCapacity, _playerRating);

        _observable = new Dictionary<Position, Planet>();
        _readOnlyObservable = new ReadOnlyDictionary<Position, Planet>(_observable);

        _altObservable = new Dictionary<Position, Planet>();
        _readOnlyAltObservable = new ReadOnlyDictionary<Position, Planet>(_altObservable);
    }

    public State Init()
    {
        _zoom = _conf.MinZoom;
        var offset = _zoom / 2;

        _leftX = _playerPosition.X - offset;
        _bottomY = _playerPosition.Y - offset;
        _altLeftX = _leftX;
        _altBottomY = _bottomY;

        _spaceGrid.Traverse(_leftX, _leftX + _zoom, _bottomY, _bottomY + _zoom, CombinedShow);

        return CurrentState();
    }

    public State Move(Direction direction)
    {
        var delta = direction.ToPosition();
        _playerPosition += delta;

        var altViewSize = _zoom;
        _spaceGrid.Traverse(_altLeftX, _altBottomY, altViewSize, direction, AltShow);
        _spaceGrid.Traverse(_altLeftX, _altBottomY, altViewSize, direction.ToOpposite(), AltHide);
        _altLeftX += delta.X;
        _altBottomY += delta.Y;

        var regularViewSize = Math.Min(_conf.AlternativeViewThreshold, _zoom);
        _spaceGrid.Traverse(_leftX, _bottomY, regularViewSize, direction, Show);
        _spaceGrid.Traverse(_leftX, _bottomY, regularViewSize, direction.ToOpposite(), Hide);
        _leftX += delta.X;
        _bottomY += delta.Y;

        return CurrentState();
    }

    public State Zoom(bool inside)
    {
        if (!CanZoom(inside)) return CurrentState();

        var targetZoom = inside ? _zoom - 1 : _zoom;

        var action = inside ? (Action<Position>) CombinedHide : CombinedShow;

        if (targetZoom % 2 == 0)
        {
            var zoomOffset = inside ? -1 : 0;
            _spaceGrid.TraverseTopToRight(_leftX, _bottomY, _zoom + zoomOffset, action);
        }
        else
        {
            var offset = (inside ? 1 : 0);
            _spaceGrid.TraverseBottomToLeft(_leftX + offset, _bottomY + offset, _zoom, action);

            var delta = inside ? 1 : -1;
            if (_zoom <= _conf.AlternativeViewThreshold)
            {
                _leftX += delta;
                _bottomY += delta;
            }

            _altLeftX += delta;
            _altBottomY += delta;
        }

        _zoom += inside ? -1 : 1;

        return CurrentState();
    }


    private bool CanZoom(bool inside)
    {
        var delta = inside ? -1 : 1;

        return _conf.MinZoom <= (_zoom + delta) && (_zoom + delta) <= _conf.MaxZoom;
    }

    private State CurrentState()
    {
        var isRegularView = _zoom < _conf.AlternativeViewThreshold;

        return new State(
            _zoom,
            _playerRating,
            isRegularView,
            _playerPosition,
            isRegularView ? _readOnlyObservable : _readOnlyAltObservable
        );
    }
}