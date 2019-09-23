using UnityEngine;

public class PlanetView : MonoBehaviour
{
    [SerializeField] private TextMesh ratingTextMesh;
    [SerializeField] private Transform planetTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(Position position, Planet planet)
    {
        spriteRenderer.color = planet.Color;
        ratingTextMesh.text = planet.Rating.ToString();
        planetTransform.position = new Vector3(position.x, position.y);
    }
}