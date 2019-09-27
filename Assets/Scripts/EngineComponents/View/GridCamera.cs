using System;
using UnityEngine;

namespace EngineComponents.View
{
    public class GridCamera : MonoBehaviour
    {
        [SerializeField] private Camera cam = default;
        private int _currentGridSize;
        private float _currentAspect;
        public float OrthographicSize => cam.orthographicSize;

        private void Awake()
        {
            _currentAspect = cam.aspect;
        }

        private void LateUpdate()
        {
            if (Math.Abs(cam.aspect - _currentAspect) <= Mathf.Epsilon) return;
            Adjust(_currentGridSize);
            _currentAspect = cam.aspect;
        }

        public void Adjust(int gridSize)
        {
            _currentGridSize = gridSize;
            var requiredSize = Mathf.CeilToInt(gridSize / 2F) + 1;
            if (cam.aspect < 1.0F)
            {
                requiredSize = Mathf.CeilToInt(requiredSize / cam.aspect);
            }

            cam.orthographicSize = requiredSize;
        }
    }
}