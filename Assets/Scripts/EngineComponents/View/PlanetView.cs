using Core.Model.Space;
using UnityEngine;

namespace EngineComponents.View
{
    public class PlanetView : MonoBehaviour
    {
        [SerializeField] private TextMesh ratingTextMesh = default;
        [SerializeField] private Transform planetTransform = default;
        [SerializeField] private SpriteRenderer spriteRenderer = default;

        public void Init(Vector3 position, Planet planet)
        {
            spriteRenderer.color = planet.Color.ToUnityEngineColor();
            ratingTextMesh.text = planet.Rating.ToString();
            planetTransform.position = position;
        }
    }
}