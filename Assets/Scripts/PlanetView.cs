using UnityEngine;

public class PlanetView : MonoBehaviour
{
    [SerializeField] private TextMesh ratingTextMesh;
    [SerializeField] private Transform planetTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(Planet planet)
    {
        spriteRenderer.color = planet.Color;
        planetTransform.position = planet.Position;
        ratingTextMesh.text = planet.Rating.ToString();
    }
}