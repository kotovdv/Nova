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
        private readonly SortedDictionary<RatingDiff, ISet<Position>> _storage;

        public AltObservableSet(int capacity, int playerRating)
        {
            _capacity = capacity;
            _playerRating = playerRating;
            _currentlyVisibleBuffer = new List<Position>(_capacity);
            _storage = new SortedDictionary<RatingDiff, ISet<Position>>(RatingDiff.AscComparator);
        }

        public void Add(Position position, int planetRating)
        {
            var ratingDiff = CreateRatingDiff(position, planetRating);

            var set = _storage.GetOrCompute(ratingDiff, () => new HashSet<Position>());
            set.Add(position);
        }

        public void Remove(Position position, int planetRating)
        {
            var ratingDiff = CreateRatingDiff(position, planetRating);

            var set = _storage.GetOrDefault(ratingDiff);
            if (set == null) return;

            set.Remove(position);
            if (set.Count == 0)
            {
                _storage.Remove(ratingDiff);
            }
        }

        public IEnumerable<Position> CurrentlyVisible()
        {
            _currentlyVisibleBuffer.Clear();
            var counter = 0;

            foreach (var kvp in _storage)
            {
                var positions = kvp.Value;
                foreach (var position in positions)
                {
                    if (counter == _capacity) break;

                    _currentlyVisibleBuffer.Add(position);
                    counter++;
                }
            }

            return _currentlyVisibleBuffer;
        }

        private RatingDiff CreateRatingDiff(Position position, int planetRating)
        {
            return new RatingDiff(Math.Abs(_playerRating - planetRating), position);
        }
    }
}