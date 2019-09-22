using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private Game _game;

    public GameObject shipPrefab;
    public GameObject planetPrefab;
    public Configuration configuration;

    public void Start()
    {
        var game = GameFactory.Generate(configuration);

        foreach (var currentPlanet in game.VisiblePlanets)
        {
            var planetInstance = Instantiate(planetPrefab);
            var planetView = planetInstance.GetComponent<PlanetView>();
            planetView.Init(currentPlanet);
        }

        var shipInstance = Instantiate(shipPrefab);
        var shipView = shipInstance.GetComponent<ShipView>();
        shipView.Init(game);
    }
}