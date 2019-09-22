using UnityEngine;

public class SpaceBootstrap : MonoBehaviour
{
    private Space _space;

    public GameObject shipPrefab;
    public GameObject planetPrefab;
    public SpaceConfiguration SpaceConfiguration;

    public void Start()
    {
        var planets = Space.Generate(SpaceConfiguration);

        foreach (var currentPlanet in planets)
        {
            var planetInstance = Instantiate(planetPrefab);
            var planetView = planetInstance.GetComponent<PlanetView>();
            planetView.Init(currentPlanet);
        }
    }
}