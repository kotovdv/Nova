using Core.Model.Game;

namespace Core
{
    public interface IGame
    {
        /// <summary>
        /// Zooms the world in our out depending on the inside value.
        /// Zoom inside makes the visible area smaller for 1 unit (10x10 -> 9x9). 
        /// Zoom outside makes the visible area bigger for 1 unit (10x10 -> 11x11).
        /// </summary>
        /// <param name="inside">Zoom inside or outside</param>
        /// <returns>New state of the game.</returns>
        State Zoom(bool inside);

        /// <summary>
        /// Moves the ship and the visible area around him in given direction.
        /// </summary>
        /// <param name="direction">Direction to move to.</param>
        /// <returns>New state of the game.</returns>
        State Move(Direction direction);
    }
}