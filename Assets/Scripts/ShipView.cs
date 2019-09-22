using UnityEngine;

public class ShipView : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;
    [SerializeField] private Transform shipTransform;

    private IGame _game;

    public void Init(IGame game)
    {
        _game = game;
        var spaceShip = game.Ship;
        textMesh.text = spaceShip.Rating.ToString();
        shipTransform.position = spaceShip.Position;
    }

    void Update()
    {
        Direction direction = Direction.None;

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