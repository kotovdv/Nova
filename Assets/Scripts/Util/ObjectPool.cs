using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly GameObject _prefab;
    private readonly Stack<GameObject> _stack;

    public ObjectPool(GameObject prefab, int capacity, int initialSize)
    {
        _prefab = prefab;
        _stack = new Stack<GameObject>(capacity);

        for (var i = 0; i < initialSize; i++)
        {
            _stack.Push(Object.Instantiate(prefab));
        }
    }

    public GameObject Borrow()
    {
        return _stack.Pop();
    }

    public void Return(GameObject gameObject)
    {
        _stack.Push(gameObject);
    }
}