namespace Core.Model.Space
{
    public readonly struct Planet
    {
        public readonly int Rating;
        public readonly Color Color;

        public Planet(int rating, Color color)
        {
            Rating = rating;
            Color = color;
        }

        public override string ToString()
        {
            return $"{nameof(Rating)}: {Rating}, {nameof(Color)}: {Color}";
        }
    }
}