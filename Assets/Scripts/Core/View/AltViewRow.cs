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

        public AddResult Add(Position position, Planet planet)
        {
            var key = new AltViewRowKey(planet.RatingDelta, position);

            var initialDelta = GetCurrentMinDelta();

            _storage.Add(key);
            if (_storage.Count > _capacity)
            {
                var max = _storage.Max;
                _storage.Remove(max);
            }

            var currentDelta = GetCurrentMinDelta();

            return new AddResult
            {
                InitialMinDelta = initialDelta,
                CurrentMinDelta = currentDelta.Value
            };
        }

        public RemoveResult Remove(Position position, Planet planet)
        {
            var key = new AltViewRowKey(planet.RatingDelta, position);

            var initialDelta = GetCurrentMinDelta();
            var removed = _storage.Remove(key);
            var currentDelta = GetCurrentMinDelta();

            return new RemoveResult
            {
                InitialMinDelta = initialDelta,
                Removed = removed,
                CurrentMinDelta = currentDelta,
            };
        }

        public void PeekTopValues(IList<Position> positions, int requiredAmount)
        {
            var currentAmount = 0;
            var targetDelta = _storage.Min.RatingDelta;

            foreach (var currentKey in _storage)
            {
                if (currentAmount == requiredAmount) break;
                if (currentKey.RatingDelta != targetDelta) break;

                positions.Add(currentKey.Position);
                currentAmount++;
            }
        }

        private int? GetCurrentMinDelta()
        {
            return _storage.Count > 0
                ? _storage.Min.RatingDelta
                : (int?) null;
        }

        public bool IsEmpty()
        {
            return _storage.Count == 0;
        }

        public struct AddResult
        {
            public int? InitialMinDelta;
            public int CurrentMinDelta;
        }

        public struct RemoveResult
        {
            public bool Removed;
            public int? InitialMinDelta;
            public int? CurrentMinDelta;
        }
    }
}