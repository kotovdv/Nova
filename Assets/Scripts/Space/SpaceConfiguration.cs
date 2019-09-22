using System;
using UnityEngine;

[Serializable]
public class SpaceConfiguration
{
    //Min value for zoom.
    public int minN;

    //Max value for zoom.
    public int maxN;

    //Density of planets. 0 - no planets. 1 - each cell contains a planet.
    [Range(0, 1)]
    public float density;

    //Minimum possible rating of an object.
    public int minRating;

    //Maximum possible rating of an object
    public int maxRating;

    //N threshold from where view is switched to ranking based.
    public int ratingViewThresholdValue;

    //Maximum amount of planets that can be displayed simultaneously in ranking based view.
    public int ratingViewMaxPlanetsValue;
}