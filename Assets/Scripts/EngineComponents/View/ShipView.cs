using UnityEngine;

namespace EngineComponents.View
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private TextMesh textMesh = default;
        [SerializeField] private Transform shipTransform = default;

        public void Init(int playerRating)
        {
            textMesh.text = playerRating.ToString();
            shipTransform.position = new Vector3(0, 0);
        }
    }
}