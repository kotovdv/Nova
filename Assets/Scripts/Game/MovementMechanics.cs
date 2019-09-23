using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementMechanics
{
    private readonly int _minZoom;
    private readonly int _maxZoom;
    private readonly int _zoomThreshold;

    private readonly SpaceGrid _spaceGrid;
    private readonly IDictionary<Position, Planet> _observablePlanets = new Dictionary<Position, Planet>();

    private int _zoom;
    private int _leftX, _bottomY;
    private Position _playerPosition;

    public MovementMechanics(
        Position playerPosition,
        SpaceGrid spaceGrid,
        int minZoom,
        int maxZoom,
        int zoomThreshold)
    {
        _zoom = minZoom;
        _spaceGrid = spaceGrid;
        _minZoom = minZoom;
        _maxZoom = maxZoom;
        _zoomThreshold = zoomThreshold;
        _playerPosition = playerPosition;
    }

    public int Zoom => _zoom;
    public Position PlayerPosition => _playerPosition;
    public IDictionary<Position, Planet> ObservablePlanets => _observablePlanets;

    public void Init()
    {
        var offset = _zoom / 2;

        _leftX = _playerPosition.X - offset;
        var rightX = _leftX + _zoom;

        _bottomY = _playerPosition.Y - offset;
        var topY = _bottomY + _zoom;

        _observablePlanets.Clear();

        for (var x = _leftX; x < rightX; x++)
        for (var y = _bottomY; y < topY; y++)
        {
            var planet = _spaceGrid[x, y];
            if (planet.HasValue)
            {
                _observablePlanets.Add(new Position(x, y), planet.Value);
            }
        }
    }

    //Get rid of a square around current view
    public void ZoomIn()
    {
        RemoveColumn(_leftX);
        RemoveColumn(_leftX + _zoom - 1);
        RemoveRow(_bottomY);
        RemoveRow(_bottomY + _zoom - 1);

        _leftX++;
        _bottomY++;
        _zoom -= 2;
    }

    //Add a square around current view
    public void ZoomOut()
    {
        AddColumn(_bottomY - 1, _bottomY + _zoom, _leftX - 1);
        AddColumn(_bottomY - 1, _bottomY + _zoom, _leftX + _zoom);

        AddRow(_leftX - 1, _leftX + _zoom, _bottomY - 1);
        AddRow(_leftX - 1, _leftX + _zoom, _bottomY + _zoom);

        _leftX--;
        _bottomY--;
        _zoom += 2;
    }


    public Position MovePlayer(Direction direction)
    {
        _playerPosition += direction.ToPosition();

        if (direction == Direction.Up || direction == Direction.Down)
        {
            var add = direction == Direction.Up ? _bottomY + _zoom : _bottomY - 1;
            var remove = direction == Direction.Up ? _bottomY : _bottomY + _zoom - 1;

            AddRow(_leftX, _leftX + _zoom, add);
            RemoveRow(remove);
            _bottomY += direction == Direction.Up ? 1 : -1;
        }
        else
        {
            var add = direction == Direction.Right ? _leftX + _zoom : _leftX - 1;
            var remove = direction == Direction.Right ? _leftX : _leftX + _zoom - 1;
            AddColumn(_bottomY, _bottomY + _zoom, add);
            RemoveColumn(remove);
            _leftX += direction == Direction.Right ? 1 : -1;
        }

        return _playerPosition;
    }


    private void AddRow(int fromX, int toX, int y)
    {
        for (var x = fromX; x < toX; x++)
        {
            var planet = _spaceGrid[x, y];
            if (planet.HasValue)
            {
                _observablePlanets[new Position(x, y)] = planet.Value;
            }
        }
    }

    private void AddColumn(int fromY, int toY, int x)
    {
        for (var y = fromY; y < toY; y++)
        {
            var planet = _spaceGrid[x, y];
            if (planet.HasValue)
            {
                _observablePlanets[new Position(x, y)] = planet.Value;
            }
        }
    }

    private void RemoveRow(int y)
    {
        var planetsInRow = _observablePlanets.Where(kvp => kvp.Key.Y == y).Select(kvp => kvp.Key).ToList();
        foreach (var position in planetsInRow)
        {
            _observablePlanets.Remove(position);
        }
    }

    private void RemoveColumn(int x)
    {
        var planetsInRow = _observablePlanets.Where(kvp => kvp.Key.X == x).Select(kvp => kvp.Key).ToList();
        foreach (var position in planetsInRow)
        {
            _observablePlanets.Remove(position);
        }
    }
}