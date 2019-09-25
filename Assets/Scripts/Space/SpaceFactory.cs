using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpaceFactory
{
    private readonly int _playerRating;
    private readonly int _planetsPerTile;
    private readonly Configuration _conf;

    public SpaceFactory(int playerRating, Configuration conf)
    {
        _conf = conf;
        _playerRating = playerRating;
        _planetsPerTile = Mathf.CeilToInt(_conf.Density * (_conf.TileSize * _conf.TileSize));
    }

    public SpaceGrid CreateGrid(Position spawnPosition)
    {
        var tile = CreateTile();
        var spaceGrid = new SpaceGrid(_conf, tile, this, spawnPosition);

        return spaceGrid;
    }

    public SpaceTile CreateTile()
    {
        var storage = new Planet?[_conf.TileSize, _conf.TileSize];
        var closestToPlayerStorage = new Position[_conf.AlternativeViewCapacity];

        PopulateTile(storage);
        ShuffleTile(storage);
        PopulateClosestPlanetsByRating(storage, closestToPlayerStorage);

        return new SpaceTile(storage, closestToPlayerStorage);
    }

    private void PopulateTile(Planet?[,] storage)
    {
        var rnd = ThreadLocalRandom.Current();

        var currentCount = 0;
        for (var i = 0; i < _conf.TileSize; i++)
        for (var j = 0; j < _conf.TileSize; j++)
        {
            if (currentCount == _planetsPerTile) break;

            var planetRating = rnd.Next(_conf.MinRating, _conf.MaxRating);
            var color = Random.ColorHSV();
            var planet = new Planet(planetRating, color);
            storage[i, j] = planet;

            currentCount++;
        }
    }

    private void ShuffleTile(Planet?[,] storage)
    {
        var rnd = ThreadLocalRandom.Current();

        for (var i = storage.Length - 1; i > 0; i--)
        {
            var i0 = i / _conf.TileSize;
            var i1 = i % _conf.TileSize;

            var j = rnd.Next(i + 1);
            var j0 = j / _conf.TileSize;
            var j1 = j % _conf.TileSize;

            var temp = storage[i0, i1];
            storage[i0, i1] = storage[j0, j1];
            storage[j0, j1] = temp;
        }
    }

    private void PopulateClosestPlanetsByRating(Planet?[,] storage, Position[] closestToPlayerStorage)
    {
        var temp = new List<RatingDiff>(_planetsPerTile);
        
        for (var i = 0; i < _conf.TileSize; i++)
        for (var j = 0; j < _conf.TileSize; j++)
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