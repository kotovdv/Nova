using System;
using Newtonsoft.Json;

namespace Core.Model.Space.Tiles
{
    [Serializable]
    public struct SpaceTile
    {
        [JsonProperty]
        private Planet?[,] _planets;

        public SpaceTile(Planet?[,] planets)
        {
            _planets = planets;
        }

        public Planet? this[int x, int y] => GetValue(x, y);

        private Planet? GetValue(int x, int y)
        {
            return _planets[x, y];
        }
    }
}