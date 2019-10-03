using System.Collections;
using System.Collections.Generic;

namespace Core.Util
{
    public class LinkedHashSet<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> _list = new LinkedList<T>();
        private readonly IDictionary<T, LinkedListNode<T>> _dict = new Dictionary<T, LinkedListNode<T>>();

        public bool Add(T elem)
        {
            if (_dict.ContainsKey(elem)) return false;

            var node = _list.AddLast(elem);
            _dict[elem] = node;

            return true;
        }

        public bool Remove(T elem)
        {
            if (!_dict.TryGetValue(elem, out var node)) return false;

            _dict.Remove(elem);
            _list.Remove(node);

            return true;
        }

        public bool IsEmpty()
        {
            return _dict.Count == 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}