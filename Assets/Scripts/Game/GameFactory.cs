using System;
using UnityEngine;
using Random = System.Random;

public class GameFactory
{
    public static IGame Generate(Configuration configuration)
    {
        //todo not exactly correct
        var maximumObservablePlanets = Mathf.CeilToInt(Math.Max(
            configuration.alternativeViewCapacity,
            configuration.alternativeViewThreshold * configuration.alternativeViewThreshold
        ));

        var spaceFactory = new SpaceFactory(
            maximumObservablePlanets,
            configuration.density,
            configuration.minRating,
            configuration.maxRating
        );

        var spaceGrid = spaceFactory.CreateGrid();

        var rating = ThreadLocalRandom.Current().Next(configuration.minRating, configuration.maxRating);
        var spawnPosition = spaceGrid.GetSpawnPosition();

        return new Game(
            rating,
            spawnPosition,
            configuration.minN,
            spaceGrid,
            maximumObservablePlanets,
            configuration
        );
    }
}