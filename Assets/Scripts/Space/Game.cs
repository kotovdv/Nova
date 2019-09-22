using UnityEngine;

public class Game : IGame
{
    private Ship _ship;
    private Planet[] _planets;
    private Vector2 _playerPosition;

    private int _currentZoom;

    public Game(Ship ship, int currentZoom, Planet[] planets, Vector2 playerPosition)
    {
        _ship = ship;
        _currentZoom = currentZoom;
        _planets = planets;
        _playerPosition = playerPosition;
    }

    public Ship Ship => _ship;
    public Planet[] VisiblePlanets => _planets;

    public Vector2 Move(Direction direction)
    {
        _ship.Position += direction.ToVector2();
        
        return _ship.Position;
    }
}