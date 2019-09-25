public interface IGame
{
    State Zoom(bool inside);
    State Move(Direction direction);
}