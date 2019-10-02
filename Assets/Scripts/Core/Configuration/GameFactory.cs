using Core.Model.Game;
using Core.Model.Space;
using Core.Model.Space.Grid;
using Core.Model.Space.Grid.IO;
using Core.Model.Space.Tiles;
using Core.Util;
using UnityEngine;

namespace Core.Configuration
{
    public static class GameFactory
    {
        private static readonly int CacheThreadsCount = Mathf.CeilToInt(SystemInfo.processorCount / 2F);

        public static (IGame, State) Generate(GameConfiguration conf)
        {
            var playerRating = ThreadLocalRandom
                .Current()
                .Next(conf.MinRating, conf.MaxRating);

            var tileFactory = SpaceTileFactory.Construct(playerRating, conf);

            var playerPosition = new Position(0, 0);
            var spaceTileIO = new SpaceTileIO(Application.persistentDataPath);

            var spaceGridTileCache = new SpaceGridTileCache(CacheThreadsCount, spaceTileIO, tileFactory);
            spaceGridTileCache.Init();

            var navigator = new SpaceGridNavigator(conf.TileSize);
            var grid = new SpaceGrid(navigator, spaceGridTileCache);

            var visibilityManager = new SpaceGridTilesVisibilityManager(conf.TileSize, navigator, spaceGridTileCache);

            visibilityManager.Init(navigator.FindTile(playerPosition));
            visibilityManager.SubscribeOnShowTile(spaceGridTileCache.LoadAsync);
            visibilityManager.SubscribeOnHideTile(spaceGridTileCache.UnloadAsync);

            var game = new Game(playerRating, grid, visibilityManager, conf);
            var initialState = game.Init(playerPosition);

            return (game, initialState);
        }
    }
}