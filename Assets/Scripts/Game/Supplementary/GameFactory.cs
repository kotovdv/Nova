public class GameFactory
{
    public static (IGame, State) Generate(Configuration configuration)
    {
        var spaceFactory = new SpaceFactory(
            configuration.MaximumObservablePlanets,
            configuration.Density,
            configuration.MinRating,
            configuration.MaxRating
        );

        var (grid, playerPos) = spaceFactory.CreateGrid();

        var rating = ThreadLocalRandom
            .Current()
            .Next(configuration.MinRating, configuration.MaxRating);

        var game = new Game(
            rating,
            playerPos,
            grid,
            configuration
        );

        var initialState = game.Init();

        return (game, initialState);
    }
}