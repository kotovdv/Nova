using UnityEngine;

public struct Planet
{
    public readonly int Rating;
    public readonly Color Color;
    public readonly Vector2 Position;
    
    public Planet(int rating, Color color, Vector2 position)
    {
        Position = position;
        Rating = rating;
        Color = color;
    }
}