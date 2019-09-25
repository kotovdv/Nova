public class GameFactory
{
    public static (IGame, State) Generate(Configuration conf)
    {
        var playerRating = ThreadLocalRandom
            .Current()
            .Next(conf.MinRating, conf.MaxRating);

        var spaceFactory = new SpaceFactory(playerRating, conf);
        var playerPosition = new Position(0, 0);
        var grid = spaceFactory.CreateGrid(playerPosition);
        var game = new Game(
            playerRating,
            playerPosition,
            grid,
            conf
        );

        var initialState = game.Init();

        return (game, initialState);
    }
}