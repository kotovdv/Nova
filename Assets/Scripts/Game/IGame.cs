using System.Collections.Generic;

public interface IGame
{
    int Zoom { get; }
    int PlayerRating { get; }
    Position PlayerPosition { get; }
    IDictionary<Position, Planet> ObservablePlanets { get; }

    void ZoomIn();
    void ZoomOut();
    Position Move(Direction direction);
}