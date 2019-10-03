using System;
using System.Collections.Generic;
using Core.Model.Game;
using Core.Model.Space;
using Core.Util;

namespace Core.View
{
    public class AltObservableSet
    {
        private readonly int _capacity;
        private readonly int _playerRating;

        private readonly IList<Position> _currentlyVisibleBuffer;
        private readonly SortedDictionary<int, ISet<Position>> _storage;

        public AltObservableSet(int capacity, int playerRating)
        {
            _capacity = capacity;
            _playerRating = playerRating;
            _currentlyVisibleBuffer = new List<Position>(_capacity);
            _storage = new SortedDictionary<int, ISet<Position>>();
        }

        public void Add(Position position, int planetRating)
        {
            var ratingDelta = CalculateDelta(planetRating);

            var set = _storage.GetOrDefault(ratingDelta);
            if (set == null)
            {
                set = new HashSet<Position>();
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
            if (set.Count == 0)
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
                //TODO order is not preserved
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