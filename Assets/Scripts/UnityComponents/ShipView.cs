using Core.Model.Space;
using UnityEngine;

namespace UnityComponents
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private TextMesh textMesh;
        [SerializeField] private Transform shipTransform;

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