using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IGame
{
    int PlayerRating { get; }
    Position PlayerPosition { get; }

    IDictionary<Position, Planet> ObservablePlanets { get; }
    int MaximumObservablePlanets { get; }

    void ZoomIn();
    void ZoomOut();
    Vector2 Move(Direction direction);
}