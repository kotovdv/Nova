using Core.Model.Game;
using Core.Model.Space;
using Core.Util;
using UnityEngine;

namespace Core.Configuration
{
    public static class GameFactory
    {
        private static readonly int TileProviderThreadsCount = Mathf.CeilToInt(SystemInfo.processorCount / 2F);

        public static (IGame, State) Generate(GameConfiguration conf)
        {
            var playerRating = ThreadLocalRandom
                .Current()
                .Next(conf.MinRating, conf.MaxRating);

            var tileProvider = ConcurrentSpaceTileProvider.Construct(
                TileProviderThreadsCount,
                conf,
                SpaceTileProvider.Construct(playerRating, conf)
            );

            var playerPosition = new Position(0, 0);
            var grid = new SpaceGrid(conf.TileSize, tileProvider);
            var game = new Game(
                playerRating,
                grid,
                conf
            );

            var initialState = game.Init(playerPosition);

            return (game, initialState);
        }
    }
}