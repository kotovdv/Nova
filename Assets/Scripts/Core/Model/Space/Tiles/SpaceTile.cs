using System;
using Newtonsoft.Json;

namespace Core.Model.Space.Tiles
{
    [Serializable]
    public class SpaceTile
    {
        [JsonProperty]
        private Planet?[,] _planets;

        public SpaceTile(Planet?[,] planets)
        {
            _planets = planets;
        }

        public ref Planet? GetValue(Position position)
        {
            return ref GetValue(position.X, position.Y);
        }

        public ref Planet? GetValue(int x, int y)
        {
            return ref _planets[x, y];
        }
    }
}