using UnityEngine;

public class SpaceFactory
{
    private readonly float _density;
    private readonly int _minRating;
    private readonly int _maxRating;

    public readonly int TileSize;

    public SpaceFactory(int tileSize, float density, int minRating, int maxRating)
    {
        TileSize = tileSize;
        _density = density;
        _minRating = minRating;
        _maxRating = maxRating;
    }

    public (SpaceGrid, Position) CreateGrid()
    {
        var tile = CreateTile();
        var offset = TileSize / 2 + 1;
        var playerPos = new Position(offset, offset);
        var spaceGrid = new SpaceGrid(tile, this);

        return (spaceGrid, playerPos);
    }

    public SpaceTile CreateTile()
    {
        var storage = new Planet?[TileSize, TileSize];
        var planetsCount = Mathf.CeilToInt(_density * (TileSize * TileSize));

        PopulateTile(storage, planetsCount);
        ShuffleTile(storage);

        return new SpaceTile(storage);
    }

    private void PopulateTile(Planet?[,] storage, int planetsCount)
    {
        var rnd = ThreadLocalRandom.Current();

        var currentCount = 0;
        for (var i = 0; i < TileSize; i++)
        for (var j = 0; j < TileSize; j++)
        {
            if (currentCount == planetsCount) break;

            var rating = rnd.Next(_minRating, _maxRating);
            var color = Random.ColorHSV();
            storage[i, j] = new Planet(rating, color);
            currentCount++;
        }
    }

    private void ShuffleTile(Planet?[,] storage)
    {
        var rnd = ThreadLocalRandom.Current();

        for (var i = storage.Length - 1; i > 0; i--)
        {
            var i0 = i / TileSize;
            var i1 = i % TileSize;

            var j = rnd.Next(i + 1);
            var j0 = j / TileSize;
            var j1 = j % TileSize;

            var temp = storage[i0, i1];
            storage[i0, i1] = storage[j0, j1];
            storage[j0, j1] = temp;
        }
    }
}