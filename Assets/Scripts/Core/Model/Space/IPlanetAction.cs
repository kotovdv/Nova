namespace Core.Model.Space
{
    public interface IPlanetAction
    {
        void Invoke(Position position, Planet planet);
    }
}