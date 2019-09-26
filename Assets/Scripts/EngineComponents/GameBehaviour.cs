using System.Collections.Generic;
using Core;
using Core.Configuration;
using Core.Model.Game;
using Core.Model.Space;
using Core.Util;
using EngineComponents.Util;
using EngineComponents.View;
using UnityEngine;

namespace EngineComponents
{
    public class GameBehaviour : MonoBehaviour
    {
        [SerializeField] private UIView uiView = default;
        [SerializeField] private GameObject shipPrefab = default;
        [SerializeField] private GameObject planetPrefab = default;
        [SerializeField] private ConfigurationScriptableObject configuration = default;

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

            var (gameInstance, initialState) = GameFactory.Generate(configuration.GameConfiguration());
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

            var mouseScrollDelta = Input.mouseScrollDelta;
            if (mouseScrollDelta == Vector2.zero) return;

            UpdateGameState(_game.Zoom(mouseScrollDelta.y > 0));
        }

        private void UpdateGameState(State state)
        {
            var zoom = state.Zoom;
            uiView.UpdateZoom(zoom);
            if (state.IsRegularView)
            {
                _gridCamera.Adjust(zoom);
            }

            var playerPosition = state.PlayerPosition;
            uiView.UpdatePosition(playerPosition);
            _shipView.SetPosition(playerPosition);

            foreach (var posPlanet in _planetViews)
            {
                _planetsPool.Return(posPlanet.Value);
            }

            _planetViews.Clear();

            foreach (var posPlanet in state.VisiblePlanets)
            {
                var planetView = _planetViews.GetOrCompute(posPlanet.Key, () => _planetsPool.Borrow());
                planetView.gameObject.SetActive(true);

                var visiblePosition = new Vector3(posPlanet.Key.X, posPlanet.Key.Y);
                if (!state.IsRegularView)
                {
                    visiblePosition.Normalize();
                    visiblePosition *= 5;
                }

                planetView.Init(visiblePosition, posPlanet.Value);
            }
        }
    }
}