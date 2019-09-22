using System;
using System.Linq;
using UnityEngine;

public interface IGame
{
    Ship Ship { get; }
    Planet[] VisiblePlanets { get; }
    Vector2 Move(Direction direction);
}