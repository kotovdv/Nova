using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    public Text zoomText;
    public Text positionText;

    public void Init(int zoom, Position position)
    {
        zoomText.text = "ZOOM: " + zoom;
        positionText.text = "POSITION: " + position.X + "," + position.Y;
    }
}