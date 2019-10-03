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
        public static (IGame, State) Generate(GameConfiguration conf)
        {
            var playerRating = ThreadLocalRandom.Current().Next(conf.MinRating, conf.MaxRating);

            var spaceGridTileCache = new SpaceGridTileCache(
                Mathf.CeilToInt(SystemInfo.processorCount / 2F),
                new SpaceTileIO(Application.persistentDataPath),
                SpaceTileFactory.Construct(conf)
            );

            spaceGridTileCache.Init();

            var gridNavigator = new SpaceGridNavigator(conf.TileSize);

            var visibilityManager = new SpaceGridTilesVisibilityManager(
                conf.TileSize,
                gridNavigator,
                spaceGridTileCache
            );

            var playerPosition = new Position(0, 0);
            visibilityManager.Init(gridNavigator.FindTile(playerPosition));

            var game = new Game(
                playerRating,
                new SpaceGrid(gridNavigator, spaceGridTileCache),
                visibilityManager,
                conf
            );

            var initialState = game.Init(playerPosition);

            return (game, initialState);
        }
    }
}