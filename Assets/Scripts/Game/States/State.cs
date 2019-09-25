using System.Collections.Generic;

public readonly struct State
{
    public readonly int Zoom;
    public readonly int PlayerRating;
    public readonly bool IsAlternativeView;
    public readonly Position PlayerPosition;
    public readonly IReadOnlyDictionary<Position, Planet> VisiblePlanets;

    public State(
        int zoom,
        int playerRating,
        bool isAlternativeView,
        Position playerPosition,
        IReadOnlyDictionary<Position, Planet> visiblePlanets)
    {
        Zoom = zoom;
        PlayerRating = playerRating;
        IsAlternativeView = isAlternativeView;
        PlayerPosition = playerPosition;
        VisiblePlanets = visiblePlanets;
    }
}