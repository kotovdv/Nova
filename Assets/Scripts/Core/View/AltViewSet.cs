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
        private readonly SortedDictionary<int, LinkedHashSet<int>> _rowsSorted;

        public AltViewSet(int capacity)
        {
            _capacity = capacity;
            _rows = new Dictionary<int, AltViewRow>();
            _currentlyVisibleBuffer = new List<Position>(_capacity);
            _rowsSorted = new SortedDictionary<int, LinkedHashSet<int>>();
        }

        public void Add(Position position, Planet planet)
        {
            var row = GetRow(position);
            var addResult = row.Add(position, planet);

            if (addResult.InitialMinDelta.HasValue)
            {
                var initialDelta = addResult.InitialMinDelta.Value;
                //Addition changed bucket
                if (addResult.CurrentMinDelta != initialDelta)
                {
                    RemoveFromDeltaStorage(initialDelta, position);
                    AddToDeltaStorage(addResult.CurrentMinDelta, position);
                }
            }
            else
            {
                AddToDeltaStorage(addResult.CurrentMinDelta, position);
            }
        }

        public void Remove(Position position, Planet planet)
        {
            var row = GetRow(position);
            var removeResult = row.Remove(position, planet);
            if (!removeResult.Removed) return;
            if (row.IsEmpty()) _rows.Remove(position.X);

            var initialMinDelta = removeResult.InitialMinDelta;
            var currentMinDelta = removeResult.CurrentMinDelta;

            if (initialMinDelta.HasValue && !currentMinDelta.HasValue)
            {
                //Row became empty
                RemoveFromDeltaStorage(initialMinDelta.Value, position);
            }
            else if (initialMinDelta.HasValue && (initialMinDelta.Value != currentMinDelta.Value))
            {
                //Row changed bucket
                RemoveFromDeltaStorage(initialMinDelta.Value, position);
                AddToDeltaStorage(currentMinDelta.Value, position);
            }
        }

        public IEnumerable<Position> CurrentlyVisible()
        {
            _currentlyVisibleBuffer.Clear();

            foreach (var kvp in _rowsSorted)
            {
                var rowNumbers = kvp.Value;
                if (_currentlyVisibleBuffer.Count == _capacity) break;
                foreach (var rowNumber in rowNumbers)
                {
                    var currentCount = _currentlyVisibleBuffer.Count;
                    if (currentCount == _capacity) break;

                    var currentRow = _rows[rowNumber];
                    currentRow.PeekTopValues(_currentlyVisibleBuffer, _capacity - currentCount);
                }
            }

            return _currentlyVisibleBuffer;
        }

        private void AddToDeltaStorage(int ratingDelta, Position position)
        {
            var deltaStorage = GetRowsWithDelta(ratingDelta);
            deltaStorage.Add(position.X);
        }

        private void RemoveFromDeltaStorage(int ratingDelta, Position position)
        {
            var deltaStorage = GetRowsWithDelta(ratingDelta);
            deltaStorage.Remove(position.X);
            if (deltaStorage.IsEmpty())
            {
                _rowsSorted.Remove(ratingDelta);
            }
        }

        private AltViewRow GetRow(Position position)
        {
            if (_rows.TryGetValue(position.X, out var row)) return row;

            row = new AltViewRow(_capacity);
            _rows[position.X] = row;

            return row;
        }

        private LinkedHashSet<int> GetRowsWithDelta(int ratingDelta)
        {
            if (_rowsSorted.TryGetValue(ratingDelta, out var set)) return set;

            set = new LinkedHashSet<int>();
            _rowsSorted[ratingDelta] = set;

            return set;
        }
    }
}