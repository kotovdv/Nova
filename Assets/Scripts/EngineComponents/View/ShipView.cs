using Core.Model.Space;
using UnityEngine;

namespace EngineComponents.View
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private TextMesh textMesh = default;
        [SerializeField] private Transform shipTransform = default;

        public void Init(int playerRating, Position playerPosition)
        {
            SetPosition(playerPosition);
            textMesh.text = playerRating.ToString();
        }

        public void SetPosition(Position position)
        {
            shipTransform.position = new Vector3(position.X, position.Y);
        }
    }
}