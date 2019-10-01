using System;
using System.Runtime.Serialization;

namespace Core.Model.Space
{
    [Serializable]
    [DataContract]
    public readonly struct Planet
    {
        [DataMember]
        public readonly int Rating;

        [DataMember]
        public readonly Color Color;

        public Planet(int rating, Color color)
        {
            Rating = rating;
            Color = color;
        }

        public bool Equals(Planet other)
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