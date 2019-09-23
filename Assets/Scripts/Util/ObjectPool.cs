using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly Stack<GameObject> _storage;

    public static ObjectPool Construct(GameObject prefab, int size)
    {
        var stack = new Stack<GameObject>(size);

        for (var i = 0; i < size; i++)
        {
            var gameObject = Object.Instantiate(prefab);
            gameObject.SetActive(false);
            stack.Push(gameObject);
        }

        return new ObjectPool(stack);
    }

    private ObjectPool(Stack<GameObject> storage)
    {
        _storage = storage;
    }

    public GameObject Borrow()
    {
        var gameObject = _storage.Pop();
        gameObject.SetActive(true);
        return gameObject;
    }

    public void Return(GameObject gameObject)
    {
        gameObject.SetActive(false);
        _storage.Push(gameObject);
    }
}