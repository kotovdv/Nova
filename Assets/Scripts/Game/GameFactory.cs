public class GameFactory
{
    public static IGame Generate(Configuration configuration)
    {
        var spaceFactory = new SpaceFactory(
            configuration.MaximumObservablePlanets,
            configuration.density,
            configuration.minRating,
            configuration.maxRating
        );

        var spaceGrid = spaceFactory.CreateGrid();

        var rating = ThreadLocalRandom.Current().Next(configuration.minRating, configuration.maxRating);
        var spawnPosition = spaceGrid.GetSpawnPosition();

        var movementMechanics = new MovementMechanics(
            spawnPosition,
            spaceGrid,
            configuration.minN,
            configuration.maxN,
            configuration.alternativeViewThreshold
        );

        movementMechanics.Init();

        return new Game(rating, movementMechanics);
    }
}