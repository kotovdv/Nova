using System.Collections.Generic;

public readonly struct RatingDiff
{
    public static readonly IComparer<RatingDiff> AscComparerWithDuplicates = new RatingDiffDescComparer();

    public readonly int Value;
    public readonly Position Position;

    public RatingDiff(int value, Position position)
    {
        Value = value;
        Position = position;
    }
}

public class RatingDiffDescComparer : IComparer<RatingDiff>
{
    public int Compare(RatingDiff x, RatingDiff y)
    {
        if (x.Position.Equals(y.Position))
        {
            return 0;
        }

        var result = x.Value.CompareTo(y.Value);

        return result != 0
            ? result
            : -1;
    }
}