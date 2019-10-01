namespace Core.Model.Space.Grid
{
    public interface IPlanetAction
    {
        void Invoke(Position position, Planet planet);
    }
}