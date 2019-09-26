using Core.Model.Space;
using UnityEngine;
using UnityEngine.UI;

namespace EngineComponents.View
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private Text zoomText = default;
        [SerializeField] private Text positionText = default;

        public void UpdateZoom(int zoom)
        {
            zoomText.text = "ZOOM: " + zoom;
        }

        public void UpdatePosition(Position position)
        {
            positionText.text = "COORDINATES: " + position.X + "," + position.Y;
        }
    }
}