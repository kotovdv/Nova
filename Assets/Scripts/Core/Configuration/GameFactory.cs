using Core.Model.Game;
using Core.Model.Space;
using Util;

namespace Core.Configuration
{
    public static class GameFactory
    {
        public static (IGame, State) Generate(global::UnityComponents.Configuration conf)
        {
            var playerRating = ThreadLocalRandom
                .Current()
                .Next(conf.MinRating, conf.MaxRating);

            var spaceFactory = SpaceTileFactory.Construct(playerRating, conf);
            var playerPosition = new Position(0, 0);
            var grid = new SpaceGrid(conf.TileSize, spaceFactory);
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
}