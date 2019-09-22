using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GameFactory
{
    public static IGame Generate(Configuration configuration)
    {
        var planets = GenerateTile(configuration);

        var rating = new Random().Next(configuration.minRating, configuration.maxRating);
        var tileSize = configuration.minN;
        var ship = new Ship(rating, new Vector2(tileSize / 2, tileSize / 2));

        return new Game(ship, configuration.minN, planets, Vector2.zero);
    }

    private static Planet[] GenerateTile(Configuration configuration)
    {
        var tileSize = configuration.minN;
        var planetsCount = Mathf.CeilToInt(configuration.density * (tileSize * tileSize));

        var grid = new Vector2[tileSize, tileSize];

        for (var i = 0; i < tileSize; i++)
        for (var j = 0; j < tileSize; j++)
        {
            grid[i, j] = new Vector2(i, j);
        }

        var random = new Random();

        for (var i = tileSize - 1; i > 0; i--)
        for (var j = tileSize - 1; j > 0; j--)
        {
            var m = random.Next(i + 1);
            var n = random.Next(j + 1);

            var temp = grid[i, j];
            grid[i, j] = grid[m, n];
            grid[m, n] = temp;
        }

        var planets = new Planet[planetsCount];
        var planetNumber = 0;

        for (var i = 0; i < tileSize; i++)
        for (var j = 0; j < tileSize; j++)
        {
            if (planetNumber >= planets.Length) break;

            planets[planetNumber++] = new Planet(
                random.Next(configuration.minRating, configuration.maxRating),
                UnityEngine.Random.ColorHSV(),
                grid[i, j]
            );
        }

        return planets;
    }
}