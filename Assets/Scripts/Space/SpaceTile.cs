public class SpaceTile
{
    private readonly Planet?[,] _planets;

    public SpaceTile(Planet?[,] planets)
    {
        _planets = planets;
    }

    public Planet? this[int x, int y] => GetValue(x, y);

    private Planet? GetValue(int x, int y)
    {
        return _planets[x, y];
    }
}