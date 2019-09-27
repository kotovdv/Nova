using Core.Model.Space;

namespace Core.Configuration
{
    public interface ISpaceTileProvider
    {
        SpaceTile Take();
    }
}