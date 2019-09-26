using System;
using System.Collections.Generic;
using Core.Model.Game;
using Core.Model.Space;
using UnityEngine;
using Util;
using Random = System.Random;

namespace Core.Configuration
{
    public sealed class SpaceTileFactory
    {
        private readonly int _tileSize;
        private readonly int _playerRating;
        private readonly int _planetsPerTile;
        private readonly int _planetMinRating;
        private readonly int _planetMaxRating;
        private readonly int _closestToPlayerStorageSize;

        public static SpaceTileFactory Construct(int playerRating, global::UnityComponents.Configuration conf)
        {
            var planetsPerTile = Mathf.CeilToInt(conf.Density * (conf.TileSize * conf.TileSize));

            return new SpaceTileFactory(
                playerRating,
                planetsPerTile,
                conf.TileSize,
                conf.MinRating,
                conf.MaxRating,
                conf.MaximumObservablePlanets
            );
        }

        public SpaceTileFactory(
            int playerRating,
            int planetsPerTile,
            int tileSize,
            int planetMinRating,
            int planetMaxRating,
            int closestToPlayerStorageSize)
        {
            _playerRating = playerRating;
            _planetsPerTile = planetsPerTile;
            _tileSize = tileSize;
            _planetMinRating = planetMinRating;
            _planetMaxRating = planetMaxRating;
            _closestToPlayerStorageSize = closestToPlayerStorageSize;
        }

        public SpaceTile CreateTile()
        {
            var storage = new Planet?[_tileSize, _tileSize];
            var closestToPlayerStorage = new Position[_closestToPlayerStorageSize];

            var rnd = ThreadLocalRandom.Current();
            //6?
            PopulateTile(storage, rnd);
            ShuffleTile(storage, rnd);
            PopulateClosestPlanetsByRating(storage, closestToPlayerStorage);

            return new SpaceTile(storage, closestToPlayerStorage);
        }

        private void PopulateTile(Planet?[,] storage, Random rnd)
        {
            var currentCount = 0;
            for (var i = 0; i < _tileSize; i++)
            for (var j = 0; j < _tileSize; j++)
            {
                if (currentCount == _planetsPerTile) break;

                var planetRating = rnd.Next(_planetMinRating, _planetMaxRating);
                var color = UnityEngine.Random.ColorHSV();
                var planet = new Planet(planetRating, color);
                storage[i, j] = planet;

                currentCount++;
            }
        }

        private void ShuffleTile(Planet?[,] storage, Random rnd)
        {
            for (var i = storage.Length - 1; i > 0; i--)
            {
                var i0 = i / _tileSize;
                var i1 = i % _tileSize;

                var j = rnd.Next(i + 1);
                var j0 = j / _tileSize;
                var j1 = j % _tileSize;

                var temp = storage[i0, i1];
                storage[i0, i1] = storage[j0, j1];
                storage[j0, j1] = temp;
            }
        }

        private void PopulateClosestPlanetsByRating(Planet?[,] storage, Position[] closestToPlayerStorage)
        {
            var temp = new List<RatingDiff>(_planetsPerTile);

            for (var i = 0; i < _tileSize; i++)
            for (var j = 0; j < _tileSize; j++)
            {
                var optionalPlanet = storage[i, j];
                if (!optionalPlanet.HasValue) continue;
                var planet = optionalPlanet.Value;

                var ratingDiff = Math.Abs(_playerRating - planet.Rating);

                temp.Add((new RatingDiff(ratingDiff, new Position(i, j))));
            }

            temp.Sort(RatingDiff.AscComparator);

            for (var i = 0; i < closestToPlayerStorage.Length; i++)
            {
                closestToPlayerStorage[i] = temp[i].Position;
            }

            temp.Clear();
        }
    }
}