using System;
using System.Runtime.Serialization;

namespace Core.Model.Space.Grid
{
    [Serializable]
    public struct SpaceTile
    {
        [DataMember]
        private readonly Planet?[][] _planets;

        [DataMember]
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