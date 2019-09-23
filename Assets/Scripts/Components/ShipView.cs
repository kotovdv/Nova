using UnityEngine;

public class ShipView : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;
    [SerializeField] private Transform shipTransform;

    private IGame _game;

    public void Init(IGame game)
    {
        _game = game;
        textMesh.text = game.PlayerRating.ToString();
        shipTransform.position = new Vector3(game.PlayerPosition.X, game.PlayerPosition.Y);
    }
}