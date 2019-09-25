using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [SerializeField] private Text zoomText;
    [SerializeField] private Text positionText;

    public void UpdateZoom(int zoom)
    {
        zoomText.text = "ZOOM: " + zoom;
    }

    public void UpdatePosition(Position position)
    {
        positionText.text = "COORDINATES: " + position.X + "," + position.Y;
    }
}