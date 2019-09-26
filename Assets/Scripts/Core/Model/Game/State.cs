using System.Collections.Generic;
using Core.Model.Space;

namespace Core.Model.Game
{
    public readonly struct State
    {
        public readonly int Zoom;
        public readonly int PlayerRating;
        public readonly bool IsRegularView;
        public readonly Position PlayerPosition;
        public readonly IReadOnlyDictionary<Position, Planet> VisiblePlanets;

        public State(
            int zoom,
            int playerRating,
            bool isRegularView,
            Position playerPosition,
            IReadOnlyDictionary<Position, Planet> visiblePlanets)
        {
            Zoom = zoom;
            PlayerRating = playerRating;
            IsRegularView = isRegularView;
            PlayerPosition = playerPosition;
            VisiblePlanets = visiblePlanets;
        }
    }
}