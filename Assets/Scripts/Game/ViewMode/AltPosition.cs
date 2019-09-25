using System;

public readonly struct AltPosition : IComparable<AltPosition>
{
    private readonly int _ratingDiff;
    private readonly float _distance;
    public readonly Position OriginalPosition;

    public AltPosition(int ratingDiff, float distance, Position originalPosition)
    {
        _ratingDiff = ratingDiff;
        _distance = distance;
        OriginalPosition = originalPosition;
    }


    public int CompareTo(AltPosition other)
    {
        var ratingDiffComparison = other._ratingDiff.CompareTo(_ratingDiff);

        return ratingDiffComparison != 0
            ? ratingDiffComparison
            : _distance.CompareTo(other._distance);
    }
}