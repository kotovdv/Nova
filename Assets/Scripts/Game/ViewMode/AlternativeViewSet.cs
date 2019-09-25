using System;
using System.Collections.Generic;

public class AlternativeViewSet
{
    private readonly int _capacity;
    private readonly int _playerRating;

    private readonly IList<Position> _currentlyVisibleBuffer;
    private readonly SortedDictionary<RatingDiff, ISet<Position>> _storage2;

    public AlternativeViewSet(int capacity, int playerRating)
    {
        _capacity = capacity;
        _playerRating = playerRating;
        _currentlyVisibleBuffer = new List<Position>(_capacity);
        _storage2 = new SortedDictionary<RatingDiff, ISet<Position>>(RatingDiff.AscComparator);
    }

    public void Add(Position position, int planetRating)
    {
        var ratingDiff = CreateRatingDiff(position, planetRating);

        var set = _storage2.GetOrCompute(ratingDiff, () => new HashSet<Position>());
        set.Add(position);
    }

    public void Remove(Position position, int planetRating)
    {
        var ratingDiff = CreateRatingDiff(position, planetRating);

        var set = _storage2[ratingDiff];
        set.Remove(position);
    }

    public IEnumerable<Position> CurrentlyVisible()
    {
        _currentlyVisibleBuffer.Clear();
        var counter = 0;

        foreach (var kvp in _storage2)
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