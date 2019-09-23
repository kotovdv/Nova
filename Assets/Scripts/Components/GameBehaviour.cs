using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject shipPrefab = default;
    [SerializeField] private GameObject planetPrefab = default;
    [SerializeField] private Configuration configuration = default;

    private Camera _playerCamera;
    private ObjectPool _planetsPool;

    //Bootstrap game.
    public void Start()
    {
        var game = GameFactory.Generate(configuration);
        var planetsPool = ObjectPool.Construct(planetPrefab, game.MaximumObservablePlanets);

        foreach (var currentPlanet in game.ObservablePlanets)
        {
            var planetInstance = planetsPool.Borrow();
            var planetView = planetInstance.GetComponent<PlanetView>();
            planetView.Init(currentPlanet.Key, currentPlanet.Value);
        }

        var shipInstance = Instantiate(shipPrefab);
        _playerCamera = shipInstance.GetComponentInChildren<Camera>();

        var shipView = shipInstance.GetComponent<ShipView>();
        shipView.Init(game);
    }
}