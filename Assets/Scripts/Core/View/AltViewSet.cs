using System;
using System.Collections.Generic;
using Core.Model.Space;
using Core.Util;

namespace Core.View
{
    public class AltViewSet
    {
        private readonly int _capacity;
        private readonly int _playerRating;

        private readonly IList<Position> _currentlyVisibleBuffer;
        private readonly SortedDictionary<int, LinkedHashSet<Position>> _storage;

        public AltViewSet(int capacity, int playerRating)
        {
            _capacity = capacity;
            _playerRating = playerRating;
            _currentlyVisibleBuffer = new List<Position>(_capacity);
            _storage = new SortedDictionary<int, LinkedHashSet<Position>>();
        }

        public void Add(Position position, int planetRating)
        {
            var ratingDelta = CalculateDelta(planetRating);

            var set = _storage.GetOrDefault(ratingDelta);
            if (set == null)
            {
                set = new LinkedHashSet<Position>();
                _storage[ratingDelta] = set;
            }

            set.Add(position);
        }

        public void Remove(Position position, int planetRating)
        {
            var ratingDelta = CalculateDelta(planetRating);

            var set = _storage.GetOrDefault(ratingDelta);
            if (set == null) return;

            set.Remove(position);
            if (set.IsEmpty())
            {
                _storage.Remove(ratingDelta);
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

        private int CalculateDelta(int planetRating)
        {
            return Math.Abs(_playerRating - planetRating);
        }
    }
}