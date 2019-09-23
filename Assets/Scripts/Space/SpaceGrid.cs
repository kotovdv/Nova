using System;
using System.Collections.Generic;

public class SpaceGrid
{
    private static readonly Position SpawnPosition = new Position(0, 0);
    private readonly SpaceFactory _factory;
    private readonly IDictionary<Position, SpaceTile> _grid = new Dictionary<Position, SpaceTile>();

    public SpaceGrid(SpaceTile initialTile, SpaceFactory factory)
    {
        _factory = factory;
        _grid.Add(SpawnPosition, initialTile);
    }

    public Planet? this[int x, int y] => GetValue(x, y);

    private Planet? GetValue(int x, int y)
    {
        var gridRow = x / _factory.TileSize;
        var gridColumn = y / _factory.TileSize;

        var tile = _grid.GetOrCompute(new Position(gridRow, gridColumn), _factory.CreateTile);

        var tileRow = Math.Abs(x % _factory.TileSize);
        var tileColumn = Math.Abs(y % _factory.TileSize);

        return tile[tileRow, tileColumn];
    }
}