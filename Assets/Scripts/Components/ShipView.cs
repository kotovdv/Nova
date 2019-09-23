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
        shipTransform.position = new Vector3(game.PlayerPosition.x, game.PlayerPosition.y);
    }

    //TODO Add proper controls
    private void Update()
    {
        var direction = Direction.None;

        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Direction.Up;
        }
        else if ((Input.GetKeyDown(KeyCode.A)))
        {
            direction = Direction.Left;
        }
        else if (((Input.GetKeyDown(KeyCode.D))))
        {
            direction = Direction.Right;
        }
        else if ((Input.GetKeyDown(KeyCode.S)))
        {
            direction = Direction.Down;
        }

        transform.position = _game.Move(direction);
    }
}