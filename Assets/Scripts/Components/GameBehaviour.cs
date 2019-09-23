using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField] private UIView uiView = default;
    [SerializeField] private GameObject shipPrefab = default;
    [SerializeField] private GameObject planetPrefab = default;
    [SerializeField] private Configuration configuration = default;

    private IGame _game;
    private ShipView _shipView;
    private GridCamera _gridCamera;
    private ObjectPool<PlanetView> _planetsPool;
    private readonly IDictionary<Position, PlanetView> _planetViews = new Dictionary<Position, PlanetView>();

    //Bootstrap game.
    public void Start()
    {
        _planetsPool = ObjectPool<PlanetView>.Construct(
            planetPrefab,
            configuration.MaximumObservablePlanets,
            go => go.GetComponent<PlanetView>()
        );

        var (gameInstance, initialState) = GameFactory.Generate(configuration);
        _game = gameInstance;

        var shipInstance = Instantiate(shipPrefab);
        _gridCamera = shipInstance.GetComponentInChildren<GridCamera>();
        _gridCamera.Adjust(initialState.Zoom);

        _shipView = shipInstance.GetComponent<ShipView>();
        _shipView.Init(initialState.PlayerRating, initialState.PlayerPosition);

        UpdateGameState(initialState);
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
            UpdateGameState(_game.Move(direction.Value));
        }

        var delta = Input.mouseScrollDelta;

        if (delta != Vector2.zero)
        {
            var state = delta.y > 0
                ? _game.ZoomIn()
                : _game.ZoomOut();

            UpdateGameState(state);
            _gridCamera.Adjust(state.Zoom);
        }
    }

    private void UpdateGameState(State state)
    {
        uiView.Init(state.Zoom, state.PlayerPosition);
        _shipView.SetPosition(state.PlayerPosition);

        foreach (var position in state.BecameInvisible)
        {
            var planetView = _planetViews.GetOrDefault(position);

            _planetViews.Remove(position);
            _planetsPool.Return(planetView);
        }

        foreach (var kvp in state.BecameVisible)
        {
            var position = kvp.Key;
            var planet = kvp.Value;
            var planetView = _planetsPool.Borrow();

            planetView.Init(position, planet);
            _planetViews.Add(position, planetView);
        }
    }
}