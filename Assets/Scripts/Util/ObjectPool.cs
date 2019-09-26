using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly Stack<T> _storage;

        public static ObjectPool<T> Construct(GameObject prefab, int size, Func<GameObject, T> componentExtractor)
        {
            var stack = new Stack<T>(size);

            for (var i = 0; i < size; i++)
            {
                var go = Object.Instantiate(prefab);
                go.SetActive(false);
                var component = componentExtractor.Invoke(go);
                stack.Push(component);
            }

            return new ObjectPool<T>(stack);
        }

        private ObjectPool(Stack<T> storage)
        {
            _storage = storage;
        }

        public T Borrow()
        {
            var component = _storage.Pop();
            component.gameObject.SetActive(true);
            return component;
        }

        public void Return(T t)
        {
            t.gameObject.SetActive(false);
            _storage.Push(t);
        }
    }
}