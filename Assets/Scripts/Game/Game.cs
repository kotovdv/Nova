using System.Collections.Generic;
using System.Linq;

public class Game : IGame
{
    private static readonly List<Position> EmptyList = new List<Position>();
    private static readonly Dictionary<Position, Planet> EmptyDict = new Dictionary<Position, Planet>();

    private readonly int _playerRating;
    private readonly Configuration _conf;
    private readonly SpaceGrid _spaceGrid;
    private readonly IDictionary<Position, Planet> _observablePlanets;

    private int _zoom;
    private bool _isAlternativeView;
    private int _leftX, _bottomY;
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
        _observablePlanets = new Dictionary<Position, Planet>();
    }

    public State Init()
    {
        _zoom = _conf.MinZoom;
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

        return new State(
            _zoom,
            _playerRating,
            _isAlternativeView,
            _playerPosition,
            _observablePlanets,
            EmptyList
        );
    }

    public State Move(Direction direction)
    {
        _playerPosition += direction.ToPosition();

        var becameVisible = new Dictionary<Position, Planet>();
        var becameInvisible = new List<Position>();
        if (direction == Direction.Up || direction == Direction.Down)
        {
            var add = direction == Direction.Up ? _bottomY + _zoom : _bottomY - 1;
            var remove = direction == Direction.Up ? _bottomY : _bottomY + _zoom - 1;

            AddRow(_leftX, _leftX + _zoom, add, becameVisible);
            RemoveRow(remove, becameInvisible);
            _bottomY += direction == Direction.Up ? 1 : -1;
        }
        else
        {
            var add = direction == Direction.Right ? _leftX + _zoom : _leftX - 1;
            var remove = direction == Direction.Right ? _leftX : _leftX + _zoom - 1;
            AddColumn(_bottomY, _bottomY + _zoom, add, becameVisible);
            RemoveColumn(remove, becameInvisible);
            _leftX += direction == Direction.Right ? 1 : -1;
        }

        return new State(
            _zoom,
            _playerRating,
            _isAlternativeView,
            _playerPosition,
            becameVisible,
            becameInvisible
        );
    }

    public State ZoomIn()
    {
        if (_zoom - 1 <= _conf.MinZoom)
        {
            return new State(
                _zoom,
                _playerRating,
                _isAlternativeView,
                _playerPosition,
                EmptyDict,
                EmptyList
            );
        }

        var becameInvisible = new List<Position>();
        RemoveColumn(_leftX, becameInvisible);
        RemoveColumn(_leftX + _zoom - 1, becameInvisible);
        RemoveRow(_bottomY, becameInvisible);
        RemoveRow(_bottomY + _zoom - 1, becameInvisible);

        _leftX++;
        _bottomY++;
        _zoom -= 2;
        _isAlternativeView = _zoom >= _conf.AlternativeViewThreshold;

        return new State(
            _zoom,
            _playerRating,
            _isAlternativeView,
            _playerPosition,
            EmptyDict,
            becameInvisible
        );
    }

    public State ZoomOut()
    {
        if (_zoom + 1 >= _conf.MaxZoom)
        {
            return new State(
                _zoom,
                _playerRating,
                _isAlternativeView,
                _playerPosition,
                EmptyDict,
                EmptyList
            );
        }

        var becameVisible = new Dictionary<Position, Planet>();
        AddColumn(_bottomY - 1, _bottomY + _zoom, _leftX - 1, becameVisible);
        AddColumn(_bottomY - 1, _bottomY + _zoom, _leftX + _zoom, becameVisible);

        AddRow(_leftX - 1, _leftX + _zoom, _bottomY - 1, becameVisible);
        AddRow(_leftX - 1, _leftX + _zoom, _bottomY + _zoom, becameVisible);

        _leftX--;
        _bottomY--;
        _zoom += 2;
        _isAlternativeView = _zoom >= _conf.AlternativeViewThreshold;

        return new State(
            _zoom,
            _playerRating,
            _isAlternativeView,
            _playerPosition,
            becameVisible,
            EmptyList
        );
    }
    
    private void AddRow(int fromX, int toX, int y, IDictionary<Position, Planet> becameVisible)
    {
        for (var x = fromX; x < toX; x++)
        {
            var planet = _spaceGrid[x, y];
            if (!planet.HasValue) continue;

            var position = new Position(x, y);
            becameVisible[position] = planet.Value;
            _observablePlanets[position] = planet.Value;
        }
    }

    private void AddColumn(int fromY, int toY, int x, IDictionary<Position, Planet> becameVisible)
    {
        for (var y = fromY; y < toY; y++)
        {
            var planet = _spaceGrid[x, y];
            if (!planet.HasValue) continue;

            var position = new Position(x, y);
            becameVisible[position] = planet.Value;
            _observablePlanets[position] = planet.Value;
        }
    }

    private void RemoveRow(int y, ICollection<Position> becameInvisible)
    {
        var planetsInRow = _observablePlanets.Where(kvp => kvp.Key.Y == y).Select(kvp => kvp.Key).ToList();
        foreach (var position in planetsInRow)
        {
            _observablePlanets.Remove(position);
            becameInvisible.Add(position);
        }
    }

    private void RemoveColumn(int x, ICollection<Position> becameInvisible)
    {
        var planetsInRow = _observablePlanets.Where(kvp => kvp.Key.X == x).Select(kvp => kvp.Key).ToList();
        foreach (var position in planetsInRow)
        {
            _observablePlanets.Remove(position);
            becameInvisible.Add(position);
        }
    }
}