using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Space
{
    private Planet[] _planets;
    private Vector2 _playerPosition;

    public static Planet[] Generate(SpaceConfiguration spaceConfiguration)
    {
        var tileSize = spaceConfiguration.minN * 10;

        return GenerateTile(tileSize, spaceConfiguration.density);
    }

    private Space()
    {
    }

    public void GeneratePlanets()
    {
        var planet = new Planet();
    }

    private static Planet[] GenerateTile(int tileSize, float density)
    {
        var planetsCount = Mathf.CeilToInt(density * (tileSize * tileSize));

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
                random.Next(0, 12000),
                UnityEngine.Random.ColorHSV(),
                grid[i, j]
            );
        }


        return planets;
    }
}