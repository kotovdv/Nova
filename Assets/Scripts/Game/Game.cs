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
        throw new NotImplementedException();
    }

    public void ZoomOut()
    {
        throw new NotImplementedException();
    }

    public Position Move(Direction direction)
    {
        return _movementMechanics.MovePlayer(direction);
    }

    public int PlayerRating => _playerRating;
    public Position PlayerPosition => _movementMechanics.PlayerPosition;
    public IDictionary<Position, Planet> ObservablePlanets => _movementMechanics.ObservablePlanets;
}