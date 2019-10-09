using System;

namespace Core.Model.Space
{
    [Serializable]
    public readonly struct Planet
    {
        public readonly int Rating;
        public readonly Color Color;
        public readonly int RatingDelta;

        public Planet(int rating, Color color, int ratingDelta)
        {
            Rating = rating;
            Color = color;
            RatingDelta = ratingDelta;
        }

        public bool Equals(Planet other)
        {
            return Rating == other.Rating && Color.Equals(other.Color) && RatingDelta == other.RatingDelta;
        }

        public override bool Equals(object obj)
        {
            return obj is Planet other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Rating;
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                hashCode = (hashCode * 397) ^ RatingDelta;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Rating)}: {Rating}, {nameof(Color)}: {Color}, {nameof(RatingDelta)}: {RatingDelta}";
        }
    }
}