using System;
using System.Collections.Generic;

public class AlternativeViewSet
{
    private readonly int _capacity;
    private readonly int _playerRating;

    private readonly SortedSet<RatingDiff> _storage;
    private readonly IList<Position> _currentlyVisibleBuffer;

    public AlternativeViewSet(int capacity, int playerRating)
    {
        _capacity = capacity;
        _playerRating = playerRating;
        _storage = new SortedSet<RatingDiff>(RatingDiff.AscComparerWithDuplicates);
        _currentlyVisibleBuffer = new List<Position>(_capacity);
    }

    public void Add(Position position, int planetRating)
    {
        _storage.Add(CreateRatingDiff(position, planetRating));
    }

    public void Remove(Position position, int planetRating)
    {
        _storage.Remove(CreateRatingDiff(position, planetRating));
    }

    public IEnumerable<Position> CurrentlyVisible()
    {
        _currentlyVisibleBuffer.Clear();
        var counter = 0;
        foreach (var diff in _storage)
        {
            if (counter == _capacity) break;
            _currentlyVisibleBuffer.Add(diff.Position);
            counter++;
        }

        return _currentlyVisibleBuffer;
    }

    private RatingDiff CreateRatingDiff(Position position, int planetRating)
    {
        return new RatingDiff(Math.Abs(_playerRating - planetRating), position);
    }
}