using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<bool> OnMouseScroll;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var movementVector = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W))
        {
            movementVector = Vector3.up;
        }
        else if ((Input.GetKeyDown(KeyCode.A)))
        {
            movementVector = Vector3.left;
        }
        else if (((Input.GetKeyDown(KeyCode.D))))
        {
            movementVector = Vector3.right;
        }
        else if ((Input.GetKeyDown(KeyCode.S)))
        {
            movementVector = Vector3.down;
        }

        transform.position += movementVector;
    }
}