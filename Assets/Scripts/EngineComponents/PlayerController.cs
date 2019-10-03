using System;
using Core;
using Core.Model.Game;
using UnityEngine;

namespace EngineComponents
{
    public class PlayerController : MonoBehaviour
    {
        private IGame _game;
        private bool _verticalAxisInUse;
        private bool _horizontalAxisInUse;
        [SerializeField] private GameView gameView;

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

        private State? HandleZoom()
        {
            bool? inside = null;

            if (Input.touchSupported && Input.touchCount == 2)
            {
                var tOne = Input.GetTouch(1);
                var tZero = Input.GetTouch(0);

                var tZeroPrevious = tZero.position - tZero.deltaPosition;
                var tOnePrevious = tOne.position - tOne.deltaPosition;

                var oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                var currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                inside = (oldTouchDistance - currentTouchDistance) < 0;
            }
            else if (Math.Abs(MouseWheel) > Mathf.Epsilon)
            {
                inside = MouseWheel > 0;
            }

            return inside.HasValue ? _game.Zoom(inside.Value) : (State?) null;
        }

        private State? HandleMovement()
        {
            Direction? direction = null;

            var verticalAxisValueAbs = Math.Abs(VerticalAxis);
            var horizontalAxisValueAbs = Math.Abs(HorizontalAxis);

            if (!_verticalAxisInUse && verticalAxisValueAbs > Mathf.Epsilon)
            {
                _verticalAxisInUse = true;
                direction = VerticalAxis > 0 ? Direction.Up : Direction.Down;
            }

            if (!_horizontalAxisInUse && horizontalAxisValueAbs > Mathf.Epsilon)
            {
                _horizontalAxisInUse = true;
                direction = HorizontalAxis > 0 ? Direction.Right : Direction.Left;
            }

            if (verticalAxisValueAbs < Mathf.Epsilon)
            {
                _verticalAxisInUse = false;
            }

            if (horizontalAxisValueAbs < Mathf.Epsilon)
            {
                _horizontalAxisInUse = false;
            }

            return direction.HasValue
                ? _game.Move(direction.Value)
                : (State?) null;
        }
    }
}