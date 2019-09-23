using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : IGame
{
    private int _playerRating;
    private Position _playerPosition;

    private int _currentZoom;

    private readonly SpaceGrid _spaceGrid;
    private readonly Configuration _configuration;

    private IDictionary<Position, Planet> _observablePlanets;
    private readonly int _maximumObservablePlanets;

    public Game(
        int playerRating,
        Position playerPosition,
        int currentZoom,
        SpaceGrid spaceGrid,
        int maximumObservablePlanets,
        Configuration configuration)
    {
        _currentZoom = currentZoom;
        _spaceGrid = spaceGrid;
        _playerPosition = playerPosition;
        _playerRating = playerRating;
        _configuration = configuration;
        _maximumObservablePlanets = maximumObservablePlanets;
        _observablePlanets = new Dictionary<Position, Planet>(maximumObservablePlanets);
        Init();
    }

    public void Init()
    {
        var offset = _currentZoom / 2;

        var fromX = _playerPosition.x - offset;
        var toX = fromX + _currentZoom;

        var fromY = _playerPosition.y - offset;
        var toY = fromY + _currentZoom;

        _observablePlanets.Clear();
        for (var x = fromX; x < toX; x++)
        for (var y = fromY; y < toY; y++)
        {
            var planet = _spaceGrid[x, y];
            if (planet.HasValue)
            {
                _observablePlanets.Add(new Position(x, y), planet.Value);
            }
        }
    }

    public void ZoomIn()
    {
        throw new NotImplementedException();
    }

    public void ZoomOut()
    {
    }

    public Vector2 Move(Direction direction)
    {
        var vector2 = direction.ToVector2();
        _playerPosition = new Position((int) vector2.x + _playerPosition.x, (int) vector2.y + _playerPosition.y);

        return new Vector2(_playerPosition.x, _playerPosition.y);
    }

    public int PlayerRating => _playerRating;
    public Position PlayerPosition => _playerPosition;
    public IDictionary<Position, Planet> ObservablePlanets => _observablePlanets;
    public int MaximumObservablePlanets => _maximumObservablePlanets;
}