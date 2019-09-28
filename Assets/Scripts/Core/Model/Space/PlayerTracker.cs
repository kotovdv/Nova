namespace Core.Model.Space
{
    public class PlayerTracker
    {
        private readonly int _tileSize;
        private readonly SpaceGrid _spaceGrid;

        public Square AltView { get; private set; }
        public Square RegView { get; private set; }
        public Position PlayerPosition { get; private set; }

        public PlayerTracker(int tileSize, SpaceGrid spaceGrid)
        {
            _tileSize = tileSize;
            _spaceGrid = spaceGrid;
        }

        public void Init(Square altView, Square regView, Position playerPosition)
        {
            AltView = altView;
            RegView = regView;
            PlayerPosition = playerPosition;
        }

        public void UpdatePlayerPosition(Position value)
        {
            PlayerPosition = value;
        }

        public void UpdateRegView(Square value)
        {
            RegView = value;
        }

        public void UpdateAltView(Square value)
        {
            AltView = value;
        }
    }
}