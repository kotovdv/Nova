using UnityEngine;

public class ShipView : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;

    public void Init(Ship ship)
    {
        textMesh.text = ship.rating.ToString();
    }
}