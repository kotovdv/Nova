using System;
using Core;
using Core.Model.Game;
using Core.Model.Space;
using UnityEngine;

namespace EngineComponents
{
    public class PlayerController : MonoBehaviour
    {
        private IGame _game;
        private bool _verticalAxisInUse;
        private bool _horizontalAxisInUse;
        [SerializeField] private GameView gameView;
        [SerializeField] private Camera _gridCamera;

        private static float VerticalAxis => Input.GetAxisRaw("Vertical");
        private static float HorizontalAxis => Input.GetAxisRaw("Horizontal");
        private static float MouseWheel => Input.GetAxis("Mouse ScrollWheel");

        public void Init(IGame game)
        {
            _game = game;
        }

        private void Update()
        {
            var afterMovementState = HandleMovement();
            var afterZoomState = HandleZoom();

            if (afterZoomState.HasValue)
            {
                gameView.UpdateView(afterZoomState.Value);
            }
            else if (afterMovementState.HasValue)
            {
                gameView.UpdateView(afterMovementState.Value);
            }
        }

        private State? HandleMovement()
        {
            Direction? direction = null;

            var movementAxis = MovementAxis();

            if (!_verticalAxisInUse && movementAxis.Y != 0)
            {
                _verticalAxisInUse = true;
                direction = movementAxis.Y > 0 ? Direction.Up : Direction.Down;
            }

            if (!_horizontalAxisInUse && movementAxis.X != 0)
            {
                _horizontalAxisInUse = true;
                direction = movementAxis.X > 0 ? Direction.Right : Direction.Left;
            }

            if (movementAxis.Y == 0) _verticalAxisInUse = false;
            if (movementAxis.X == 0) _horizontalAxisInUse = false;

            return direction.HasValue
                ? _game.Move(direction.Value)
                : (State?) null;
        }

        private State? HandleZoom()
        {
            var zoomAxis = ZoomAxis();
            if (zoomAxis == 0)
            {
                return null;
            }

            return _game.Zoom(zoomAxis < 0);
        }

        private Position MovementAxis()
        {
            var x = 0;
            var y = 0;

            var verticalAxisValueAbs = Math.Abs(VerticalAxis);
            var horizontalAxisValueAbs = Math.Abs(HorizontalAxis);

            if (Input.touchSupported && Input.touchCount == 1)
            {
                var touchPosition = Input.GetTouch(0).position;
                var touchViewportPosition = _gridCamera.ScreenToViewportPoint(touchPosition);

                if (touchViewportPosition.x > 0.7f) x = 1;
                if (touchViewportPosition.x < 0.3f) x = -1;
                if (touchViewportPosition.y > 0.7f) y = 1;
                if (touchViewportPosition.y < 0.3f) y = -1;
            }
            else
            {
                if (verticalAxisValueAbs > 0)
                {
                    y = VerticalAxis > Mathf.Epsilon ? 1 : -1;
                }

                if (horizontalAxisValueAbs > 0)
                {
                    x = HorizontalAxis > Mathf.Epsilon ? 1 : -1;
                }
            }

            return new Position(x, y);
        }

        private static int ZoomAxis()
        {
            var value = 0;

            if (Input.touchSupported && Input.touchCount == 2)
            {
                var tOne = Input.GetTouch(1);
                var tZero = Input.GetTouch(0);

                var tZeroPrevious = tZero.position - tZero.deltaPosition;
                var tOnePrevious = tOne.position - tOne.deltaPosition;

                var oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                var currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                value = (oldTouchDistance - currentTouchDistance) < 0 ? -1 : 1;
            }
            else if (Math.Abs(MouseWheel) > Mathf.Epsilon)
            {
                value = MouseWheel > 0 ? -1 : 1;
            }

            return value;
        }
    }
}