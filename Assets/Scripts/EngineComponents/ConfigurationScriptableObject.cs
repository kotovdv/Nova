using System;
using Core.Configuration;
using UnityEngine;
using UnityEngine.Assertions;

namespace EngineComponents
{
    [CreateAssetMenu(menuName = "Nova/Configuration")]
    public class ConfigurationScriptableObject : ScriptableObject
    {
        //Min value for view distance (i.e. NxN square around the ship)
        [Min(0)]
        [SerializeField] private int minZoom;

        //Max value for view distance (i.e. NxN square around the ship)
        [Min(0)]
        [SerializeField] private int maxZoom;

        //Density of planets (0 - no planets. 1 - each cell contains a planet)
        [Range(0, 1)]
        [SerializeField] private float density;

        //Minimum possible rating of an object.
        [Min(0)]
        [SerializeField] private int minRating;

        //Maximum possible rating of an object
        [Min(0)]
        [SerializeField] private int maxRating;

        //N threshold from where view is switched to alternative rating based view.
        [Min(0)]
        [SerializeField] private int alternativeViewThreshold;

        //Maximum amount of planets that can be displayed simultaneously in rating based view.
        [Min(0)]
        [SerializeField] private int alternativeViewCapacity;

        //Space will be generated via tiles with given TileSize. Choose carefully.
        [Min(0)]
        [SerializeField] private int tileSize;

        //Maximum amount of planets that can be displayed in any game view.
        public int MaximumObservablePlanets => Mathf.CeilToInt(Math.Max(
            alternativeViewCapacity,
            alternativeViewThreshold * alternativeViewThreshold
        ));

        public GameConfiguration GameConfiguration()
        {
            return new GameConfiguration(
                minZoom,
                maxZoom,
                tileSize,
                density,
                minRating,
                maxRating,
                alternativeViewThreshold,
                alternativeViewCapacity
            );
        }

        private void OnValidate()
        {
            Assert.IsTrue(minZoom <= maxZoom, "Min view distance should be less or equal to max view distance");
            Assert.IsTrue(minRating <= maxRating, "Min possible rating should be less or equal to max possible rating");
            Assert.IsTrue(minZoom < alternativeViewThreshold && alternativeViewThreshold < maxZoom,
                "Alternative view threshold should be between min N and max N");
        }
    }
}