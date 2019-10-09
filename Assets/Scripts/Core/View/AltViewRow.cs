using System.Collections.Generic;
using Core.Model.Space;

namespace Core.View
{
    public class AltViewRow
    {
        private readonly int _capacity;
        private readonly SortedSet<AltViewRowKey> _storage;

        public AltViewRow(int capacity)
        {
            _capacity = capacity;
            _storage = new SortedSet<AltViewRowKey>();
        }

        public Position? Add(Position position, Planet planet)
        {
            var key = new AltViewRowKey(planet.RatingDelta, position);
            _storage.Add(key);

            if (_storage.Count <= _capacity) return null;

            var max = _storage.Max;
            _storage.Remove(max);

            return max.Position;
        }

        public bool Remove(Position position, Planet planet)
        {
            var key = new AltViewRowKey(planet.RatingDelta, position);

            return _storage.Remove(key);
        }

        public bool IsEmpty()
        {
            return _storage.Count == 0;
        }
    }
}