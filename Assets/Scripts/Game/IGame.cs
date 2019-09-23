public interface IGame
{
    State ZoomIn();
    State ZoomOut();
    State Move(Direction direction);
}