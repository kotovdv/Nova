using Core.Configuration;
using Core.Util;
using UnityEngine;
using Random = System.Random;

namespace Core.Model.Space.Tiles
{
    public class SpaceTileFactory
    {
        private readonly int _tileSize;
        private readonly int _planetsPerTile;
        private readonly int _planetMinRating;
        private readonly int _planetMaxRating;

        public static SpaceTileFactory Construct(GameConfiguration conf)
        {
            var planetsPerTile = Mathf.CeilToInt(conf.Density * (conf.TileSize * conf.TileSize));

            return new SpaceTileFactory(
                conf.TileSize,
                planetsPerTile,
                conf.MinRating,
                conf.MaxRating
            );
        }

        public SpaceTileFactory(int tileSize, int planetsPerTile, int planetMinRating, int planetMaxRating)
        {
            _planetsPerTile = planetsPerTile;
            _tileSize = tileSize;
            _planetMinRating = planetMinRating;
            _planetMaxRating = planetMaxRating;
        }

        public SpaceTile CreateTile()
        {
            var storage = new Planet?[_tileSize, _tileSize];

            var rnd = ThreadLocalRandom.Current();
            PopulateTile(storage, rnd);
            ShuffleTile(storage, rnd);

            return new SpaceTile(storage);
        }

        private void PopulateTile(Planet?[,] storage, Random rnd)
        {
            var currentCount = 0;
            for (var i = 0; i < _tileSize; i++)
            {
                for (var j = 0; j < _tileSize; j++)
                {
                    if (currentCount == _planetsPerTile) break;

                    var planetRating = rnd.Next(_planetMinRating, _planetMaxRating);

                    var r = (byte) rnd.Next(256);
                    var g = (byte) rnd.Next(256);
                    var b = (byte) rnd.Next(256);

                    var color = new Color(r, g, b);

                    var planet = new Planet(planetRating, color);
                    storage[i, j] = planet;

                    currentCount++;
                }
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
    }
}