using Core.Model.Space;
using UnityEngine;

namespace UnityComponents
{
    public class PlanetView : MonoBehaviour
    {
        [SerializeField] private TextMesh ratingTextMesh;
        [SerializeField] private Transform planetTransform;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void Init(Vector3 position, Planet planet)
        {
            gameObject.SetActive(true);
            spriteRenderer.color = planet.Color;
            ratingTextMesh.text = planet.Rating.ToString();
            planetTransform.position = position;
        }
    }
}