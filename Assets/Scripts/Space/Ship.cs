using UnityEngine;

public struct Ship
{
    public Vector2 Position;
    public readonly int Rating;

    public Ship(int rating, Vector2 position)
    {
        Rating = rating;
        Position = position;
    }
}