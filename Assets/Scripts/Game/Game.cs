using System;
using System.Collections.Generic;

public class Game : IGame
{
    private readonly int _playerRating;
    private readonly MovementMechanics _movementMechanics;

    public Game(
        int playerRating,
        MovementMechanics movementMechanics)
    {
        _playerRating = playerRating;
        _movementMechanics = movementMechanics;
    }

    public void ZoomIn()
    {
        _movementMechanics.ZoomIn();
    }

    public void ZoomOut()
    {
        _movementMechanics.ZoomOut();
    }

    public Position Move(Direction direction)
    {
        return _movementMechanics.MovePlayer(direction);
    }

    public int Zoom => _movementMechanics.Zoom;
    public int PlayerRating => _playerRating;
    public Position PlayerPosition => _movementMechanics.PlayerPosition;
    public IDictionary<Position, Planet> ObservablePlanets => _movementMechanics.ObservablePlanets;
}