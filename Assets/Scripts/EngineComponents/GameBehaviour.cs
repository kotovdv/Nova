using System;
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

        private int _zoom;
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
            _shipView.Init(initialState.PlayerRating);

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
            _zoom = state.Zoom;
            uiView.UpdateZoom(_zoom);
            if (state.IsRegularView)
            {
                _gridCamera.Adjust(_zoom);
            }

            var playerPosition = state.PlayerPosition;
            uiView.UpdatePosition(playerPosition);

            foreach (var posPlanet in _planetViews)
            {
                var planetView = posPlanet.Value;
                planetView.gameObject.SetActive(false);
                _planetsPool.Return(planetView);
            }

            DisplayPlanets(state, playerPosition.ToVector3());
        }

        private void DisplayPlanets(State state, Vector3 playerPosition)
        {
            _planetViews.Clear();

            var sideSquare = Mathf.Pow(_zoom / 2F, 2);
            var maxDistance = Mathf.Sqrt(sideSquare + sideSquare);

            foreach (var posPlanet in state.VisiblePlanets)
            {
                var planetPos = posPlanet.Key;
                if (!_planetViews.TryGetValue(planetPos, out var planetView))
                {
                    planetView = _planetsPool.Borrow();
                    _planetViews[planetPos] = planetView;
                }

                var planetVector = planetPos.ToVector3() - playerPosition;

                if (!state.IsRegularView)
                {
                    var scale = planetVector.magnitude / maxDistance;
                    planetVector.Normalize();
                    planetVector *= scale * _gridCamera.OrthographicSize;
                }

                planetView.gameObject.SetActive(true);
                planetView.Init(planetVector, posPlanet.Value);
            }
        }
    }
}