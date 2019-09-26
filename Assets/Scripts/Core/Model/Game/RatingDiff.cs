using System.Collections.Generic;
using Core.Model.Space;

namespace Core.Model.Game
{
    public readonly struct RatingDiff
    {
        public static readonly IComparer<RatingDiff> AscComparator = new RatingDiffDescComparer();

        public readonly int Value;
        public readonly Position Position;

        public RatingDiff(int value, Position position)
        {
            Value = value;
            Position = position;
        }

        public override string ToString()
        {
            return $"{nameof(Value)}: {Value}, {nameof(Position)}: {Position}";
        }
    }

    public class RatingDiffDescComparer : IComparer<RatingDiff>
    {
        public int Compare(RatingDiff x, RatingDiff y)
        {
            return x.Value.CompareTo(y.Value);
        }
    }
}