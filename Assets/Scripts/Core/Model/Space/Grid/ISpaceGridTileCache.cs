using Core.Model.Space.Tiles;

namespace Core.Model.Space.Grid
{
    public interface ISpaceGridTileCache
    {
        SpaceTile Get(Position position);

        void Load(Position position);
        void LoadAsync(Position position);

        void Unload(Position position);
        void UnloadAsync(Position position);
    }
}