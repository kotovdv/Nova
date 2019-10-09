using System.Collections.Generic;
using Core.Model.Space;
using Core.Util;

namespace Core.View
{
    public class AltViewSet
    {
        private readonly int _capacity;
        private readonly IDictionary<int, AltViewRow> _rows;
        private readonly IList<Position> _currentlyVisibleBuffer;
        private readonly SortedDictionary<int, LinkedHashSet<Position>> _storage;

        public AltViewSet(int capacity)
        {
            _capacity = capacity;
            _rows = new Dictionary<int, AltViewRow>();
            _currentlyVisibleBuffer = new List<Position>(_capacity);
            _storage = new SortedDictionary<int, LinkedHashSet<Position>>();
        }

        public void Add(Position position, Planet planet)
        {
            if (!_rows.TryGetValue(position.X, out var row))
            {
                row = new AltViewRow(_capacity);
                _rows[position.X] = row;
            }

            var replaced = row.Add(position, planet);
            if (!replaced.HasValue || !replaced.Value.Equals(position))
            {
                AddToStorage(position, planet);
            }
            else
            {
                RemoveFromStorage(position, planet);
            }
        }

        public void Remove(Position position, Planet planet)
        {
            if (!_rows.TryGetValue(position.X, out var row)) return;
            if (!row.Remove(position, planet)) return;

            RemoveFromStorage(position, planet);
            if (row.IsEmpty())
            {
                _rows.Remove(position.X);
            }
        }

        private void AddToStorage(Position position, Planet planet)
        {
            if (!_storage.TryGetValue(planet.RatingDelta, out var set))
            {
                set = new LinkedHashSet<Position>();
                _storage[planet.RatingDelta] = set;
            }

            set.Add(position);
        }

        private void RemoveFromStorage(Position position, Planet planet)
        {
            if (!_storage.TryGetValue(planet.RatingDelta, out var set)) return;

            set.Remove(position);
            if (set.IsEmpty())
            {
                _storage.Remove(planet.RatingDelta);
            }
        }

        public IEnumerable<Position> CurrentlyVisible()
        {
            _currentlyVisibleBuffer.Clear();
            var counter = 0;

            foreach (var kvp in _storage)
            {
                var positions = kvp.Value;
                if (counter == _capacity) break;
                foreach (var position in positions)
                {
                    if (counter == _capacity) break;
                    _currentlyVisibleBuffer.Add(position);
                    counter++;
                }
            }

            return _currentlyVisibleBuffer;
        }
    }
}