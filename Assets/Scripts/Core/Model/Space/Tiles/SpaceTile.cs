using System;
using Newtonsoft.Json;

namespace Core.Model.Space.Tiles
{
    [Serializable]
    public struct SpaceTile
    {
        [JsonProperty]
        private readonly Planet?[][] _planets;

        [JsonProperty]
        private readonly Position[] _closestPlanetsByRating;

        public SpaceTile(Planet?[][] planets, Position[] closestPlanetsByRating)
        {
            _planets = planets;
            _closestPlanetsByRating = closestPlanetsByRating;
        }

        public Planet? this[int x, int y] => GetValue(x, y);

        private Planet? GetValue(int x, int y)
        {
            return _planets[x][y];
        }
    }
}