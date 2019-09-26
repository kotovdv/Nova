using Core.Model.Game;

namespace Core
{
    public interface IGame
    {
        State Zoom(bool inside);
        State Move(Direction direction);
    }
}