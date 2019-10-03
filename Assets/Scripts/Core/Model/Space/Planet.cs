using System;

namespace Core.Model.Space
{
    [Serializable]
    public readonly struct Planet
    {
        public readonly int Rating;
        public readonly Color Color;

        public Planet(int rating, Color color)
        {
            Rating = rating;
            Color = color;
        }

        private bool Equals(Planet other)
        {
            return Rating == other.Rating && Color.Equals(other.Color);
        }

        public override bool Equals(object obj)
        {
            return obj is Planet other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Rating * 397) ^ Color.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"{nameof(Rating)}: {Rating}, {nameof(Color)}: {Color}";
        }
    }
}