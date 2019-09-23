using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject shipPrefab = default;
    [SerializeField] private GameObject planetPrefab = default;
    [SerializeField] private Configuration configuration = default;

    private IGame _game;
    private Camera _playerCamera;
    private ObjectPool _planetsPool;

    private ShipView _shipView;
    private IList<PlanetView> _planetViews = new List<PlanetView>();

    //Bootstrap game.
    public void Start()
    {
        _game = GameFactory.Generate(configuration);
        _planetsPool = ObjectPool.Construct(planetPrefab, configuration.MaximumObservablePlanets);

        foreach (var currentPlanet in _game.ObservablePlanets)
        {
            var planetInstance = _planetsPool.Borrow();
            var planetView = planetInstance.GetComponent<PlanetView>();
            planetView.Init(currentPlanet.Key, currentPlanet.Value);
            _planetViews.Add(planetView);
        }

        var shipInstance = Instantiate(shipPrefab);
        _playerCamera = shipInstance.GetComponentInChildren<Camera>();

        var shipView = shipInstance.GetComponent<ShipView>();
        shipView.Init(_game);
        _shipView = shipView;
    }

    private void Update()
    {
        Direction? direction = null;

        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Direction.Up;
        }
        else if ((Input.GetKeyDown(KeyCode.A)))
        {
            direction = Direction.Left;
        }
        else if (((Input.GetKeyDown(KeyCode.D))))
        {
            direction = Direction.Right;
        }
        else if ((Input.GetKeyDown(KeyCode.S)))
        {
            direction = Direction.Down;
        }

        if (!direction.HasValue) return;

        foreach (var currentPlanet in _planetViews)
        {
            _planetsPool.Return(currentPlanet.gameObject);
        }

        _planetViews.Clear();

        var position = _game.Move(direction.Value);
        _shipView.gameObject.transform.position = new Vector3(position.X, position.Y);

        foreach (var currentPlanet in _game.ObservablePlanets)
        {
            var planetInstance = _planetsPool.Borrow();
            var planetView = planetInstance.GetComponent<PlanetView>();
            planetView.Init(currentPlanet.Key, currentPlanet.Value);
            _planetViews.Add(planetView);
        }
    }
}