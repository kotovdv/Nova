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

    public Planet GetPlanet(Position position)
    {
        var optional = TryGetPlanet(position);
        if (!optional.HasValue)
        {
            throw new NullReferenceException("No planet at [" + position + "]");
        }

        return optional.Value;
    }

    public Planet? TryGetPlanet(Position position)
    {
        var gridRow = position.X / _factory.TileSize;
        var gridColumn = position.Y / _factory.TileSize;

        var tile = _grid.GetOrCompute(new Position(gridRow, gridColumn), _factory.CreateTile);

        var tileRow = Math.Abs(position.X % _factory.TileSize);
        var tileColumn = Math.Abs(position.Y % _factory.TileSize);

        return tile[tileRow, tileColumn];
    }

    public void Traverse(int leftX, int bottomY, int size, Direction direction, Action<Position> action)
    {
        Traverse(leftX, bottomY, size, size, direction, action);
    }

    public void Traverse(int leftX, int bottomY, int sizeX, int sizeY, Direction direction, Action<Position> action)
    {
        switch (direction)
        {
            case Direction.Left:
                TraverseVertical(bottomY, bottomY + sizeY, leftX - 1, action);
                break;
            case Direction.Right:
                TraverseVertical(bottomY, bottomY + sizeY, leftX + sizeX, action);
                break;
            case Direction.Up:
                TraverseHorizontal(leftX, leftX + sizeX, bottomY + sizeY, action);
                break;
            case Direction.Down:
                TraverseHorizontal(leftX, leftX + sizeX, bottomY - 1, action);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    public void Traverse(int fromX, int toX, int fromY, int toY, Action<Position> action)
    {
        for (var x = fromX; x < toX; x++)
        {
            for (var y = fromY; y < toY; y++)
            {
                var position = new Position(x, y);

                var optPlanet = TryGetPlanet(position);
                if (!optPlanet.HasValue) continue;

                action.Invoke(position);
            }
        }
    }

    private void TraverseVertical(int fromY, int toY, int x, Action<Position> action)
    {
        Traverse(x, x + 1, fromY, toY, action);
    }

    private void TraverseHorizontal(int fromX, int toX, int y, Action<Position> action)
    {
        Traverse(fromX, toX, y, y + 1, action);
    }
}