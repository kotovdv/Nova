using System;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(menuName = "Nova/SpaceConfiguration")]
public class Configuration : ScriptableObject
{
    //Min value for view distance (i.e. NxN square around the ship)
    [Min(0)]
    public int minN;

    //Max value for view distance (i.e. NxN square around the ship)
    [Min(0)]
    public int maxN;

    //Density of planets (0 - no planets. 1 - each cell contains a planet)
    [Range(0, 1)]
    public float density;

    //Minimum possible rating of an object.
    [Min(0)]
    public int minRating;

    //Maximum possible rating of an object
    [Min(0)]
    public int maxRating;

    //N threshold from where view is switched to alternative rating based view.
    [Min(0)]
    public int alternativeViewThreshold;

    //Maximum amount of planets that can be displayed simultaneously in rating based view.
    [Min(0)]
    public int alternativeViewCapacity;

    private void OnValidate()
    {
        Assert.IsTrue(minN <= maxN, "Min view distance should be less or equal to max view distance");
        Assert.IsTrue(minRating <= maxRating, "Min possible rating should be less or equal to max possible rating");
        Assert.IsTrue(minN < alternativeViewThreshold && alternativeViewThreshold < maxN,
            "Alternative view threshold should be between min N and max N");
    }
}