using System;
using Core.Model.Space;

namespace Core.View
{
    public readonly struct AltViewRowKey : IComparable<AltViewRowKey>
    {
        public readonly int RatingDelta;
        public readonly Position Position;

        public AltViewRowKey(int ratingDelta, Position position)
        {
            RatingDelta = ratingDelta;
            Position = position;
        }
        
        public int CompareTo(AltViewRowKey other)
        {
            var ratingDeltaComparison = RatingDelta.CompareTo(other.RatingDelta);
            if (ratingDeltaComparison != 0) return ratingDeltaComparison;
            return Position.CompareTo(other.Position);
        }

        public override string ToString()
        {
            return $"{nameof(RatingDelta)}: {RatingDelta}, {nameof(Position)}: {Position}";
        }
    }
}