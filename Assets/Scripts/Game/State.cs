using System.Collections.Generic;

public readonly struct State
{
    public readonly int Zoom;
    public readonly int PlayerRating;
    public readonly bool IsAlternativeView;
    public readonly Position PlayerPosition;

    //TODO RESTRICT ACCESS.
    public readonly IDictionary<Position, Planet> BecameVisible;
    public readonly IList<Position> BecameInvisible;

    public State(
        int zoom,
        int playerRating,
        bool isAlternativeView,
        Position playerPosition,
        IDictionary<Position, Planet> becameVisible,
        IList<Position> becameInvisible)
    {
        Zoom = zoom;
        PlayerRating = playerRating;
        PlayerPosition = playerPosition;
        BecameVisible = becameVisible;
        BecameInvisible = becameInvisible;
        IsAlternativeView = isAlternativeView;
    }
}