using System;
using System.Collections.Generic;

public class AlternativeViewSet
{
    private readonly int _capacity;
    private readonly int _playerRating;
    private readonly SortedSet<AltPosition> _storage = new SortedSet<AltPosition>();

    public AlternativeViewSet(int capacity, int playerRating)
    {
        _capacity = capacity;
        _playerRating = playerRating;
    }

    public (bool, Position?) Add(Planet planet, Position planetPosition, Position playerPosition)
    {
        var width = (playerPosition.X - planetPosition.X);
        var height = (playerPosition.Y - planetPosition.Y);

        var altPosition = new AltPosition(
            Math.Abs(_playerRating - planet.Rating),
            height * height + width * width,
            planetPosition
        );
        
        return Add(altPosition);
    }

    public void Remove(Position position)
    {
        _storage.RemoveWhere(altPosition => altPosition.OriginalPosition.Equals(position));
    }

    private (bool, Position?) Add(AltPosition value)
    {
        if (_storage.Count < _capacity)
        {
            _storage.Add(value);
            return (true, null);
        }

        var storedMin = _storage.Min;
        if (storedMin.CompareTo(value) >= 0) return (false, null);

        _storage.Remove(storedMin);
        _storage.Add(value);

        return (true, storedMin.OriginalPosition);
    }
}