public readonly struct GameConfiguration
{
    public readonly int MinZoom;
    public readonly int MaxZoom;
    public readonly int TileSize;
    public readonly float Density;
    public readonly int MinRating;
    public readonly int MaxRating;
    public readonly int AlternativeViewThreshold;
    public readonly int AlternativeViewCapacity;
    public readonly int MaximumObservablePlanets;

    public GameConfiguration(
        int minZoom,
        int maxZoom,
        int tileSize,
        float density,
        int minRating,
        int maxRating,
        int alternativeViewThreshold,
        int alternativeViewCapacity,
        int maximumObservablePlanets
    )
    {
        MinZoom = minZoom;
        MaxZoom = maxZoom;
        TileSize = tileSize;
        Density = density;
        MinRating = minRating;
        MaxRating = maxRating;
        AlternativeViewThreshold = alternativeViewThreshold;
        AlternativeViewCapacity = alternativeViewCapacity;
        MaximumObservablePlanets = maximumObservablePlanets;
    }
}