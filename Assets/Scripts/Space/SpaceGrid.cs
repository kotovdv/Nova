using System;
using System.Collections.Generic;
using UnityEngine;

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

    public Position GetSpawnPosition()
    {
        var offset = _factory.TileSize / 2 + 1;
        return new Position(offset, offset);
    }

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