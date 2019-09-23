using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject shipPrefab = default;
    [SerializeField] private GameObject planetPrefab = default;
    [SerializeField] private Configuration configuration = default;

    private IGame _game;
    private GridCamera _gridCamera;
    private ObjectPool _planetsPool;

    private ShipView _shipView;
    private IList<PlanetView> _planetViews = new List<PlanetView>();

    //Bootstrap game.
    public void Start()
    {
        _game = GameFactory.Generate(configuration);
        //TODO CHANGE
        _planetsPool = ObjectPool.Construct(planetPrefab, 10_000);

        foreach (var currentPlanet in _game.ObservablePlanets)
        {
            var planetInstance = _planetsPool.Borrow();
            var planetView = planetInstance.GetComponent<PlanetView>();
            planetView.Init(currentPlanet.Key, currentPlanet.Value);
            _planetViews.Add(planetView);
        }

        var shipInstance = Instantiate(shipPrefab);
        _gridCamera = shipInstance.GetComponentInChildren<GridCamera>();
        _gridCamera.Adjust(_game.Zoom);

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

        if (direction.HasValue)
        {
            var position = _game.Move(direction.Value);
            _shipView.gameObject.transform.position = new Vector3(position.X, position.Y);
            RedrawPlanets();
        }

        var delta = Input.mouseScrollDelta;

        if (delta != Vector2.zero)
        {
            if (delta.y > 0)
            {
                _game.ZoomIn();
            }
            else
            {
                _game.ZoomOut();
            }

            RedrawPlanets();
            _gridCamera.Adjust(_game.Zoom);
        }
    }

    private void RedrawPlanets()
    {
        foreach (var currentPlanet in _planetViews)
        {
            _planetsPool.Return(currentPlanet.gameObject);
        }

        _planetViews.Clear();

        foreach (var currentPlanet in _game.ObservablePlanets)
        {
            var planetInstance = _planetsPool.Borrow();
            var planetView = planetInstance.GetComponent<PlanetView>();
            planetView.Init(currentPlanet.Key, currentPlanet.Value);
            _planetViews.Add(planetView);
        }
    }
}