using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model.Space.Grid.Storage;
using Core.Util;

namespace Core.Model.Space.Grid
{
    public class SpaceGridTileCache : ISpaceGridTileCache
    {
        private readonly Synchronizer<Position> _synchronizer = new Synchronizer<Position>();

        private readonly int _threadsCount;
        private readonly SpaceTileIO _tileIO;
        private readonly SpaceTileFactory _tileFactory;
        private readonly IDictionary<Position, SpaceTile?> _tiles = new Dictionary<Position, SpaceTile?>();
        private readonly IDictionary<Position, bool?> _tilesLoadStatuses = new Dictionary<Position, bool?>();
        private readonly IDictionary<Position, TaskType?> _tilesTasks = new Dictionary<Position, TaskType?>();
        private readonly BlockingCollection<Position> _positionsWithTasks = new BlockingCollection<Position>();

        public SpaceGridTileCache(int threadsCount, SpaceTileIO tileIO, SpaceTileFactory tileFactory)
        {
            _tileIO = tileIO;
            _threadsCount = threadsCount;
            _tileFactory = tileFactory;
        }

        public void Init()
        {
            for (var i = 0; i < _threadsCount; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    while (true) ConsumeTask();
                }, TaskCreationOptions.LongRunning);
            }
        }

        public SpaceTile Get(Position position)
        {
            lock (_synchronizer[position])
            {
                var tile = _tiles.GetOrDefault(position, null);
                if (tile == null)
                {
                    Load(position);
                }

                return _tiles[position].Value;
            }
        }

        public void LoadAsync(Position position)
        {
            lock (_synchronizer[position])
            {
                _tilesTasks[position] = TaskType.Load;
                _positionsWithTasks.Add(position);
            }
        }

        public void UnloadAsync(Position position)
        {
            lock (_synchronizer[position])
            {
                _tilesTasks[position] = TaskType.Unload;
                _positionsWithTasks.Add(position);
            }
        }

        public void Load(Position position)
        {
            lock (_synchronizer[position])
            {
                var isLoaded = _tilesLoadStatuses.GetOrDefault(position, null);

                if (isLoaded.HasValue && isLoaded.Value) return;

                var spaceTile = isLoaded.HasValue ? _tileIO.Read(position) : _tileFactory.CreateTile();
                _tiles[position] = spaceTile;
                _tilesLoadStatuses[position] = true;
            }
        }

        public void Unload(Position position)
        {
            lock (_synchronizer[position])
            {
                var isLoaded = _tilesLoadStatuses.GetOrDefault(position, null);

                if (!isLoaded.HasValue || !isLoaded.Value) return;

                _tiles[position] = null;
                _tilesLoadStatuses[position] = false;
            }
        }

        private void ConsumeTask()
        {
            var position = _positionsWithTasks.Take();
            lock (_synchronizer[position])
            {
                var task = _tilesTasks[position];
                if (task == TaskType.Load)
                {
                    Load(position);
                }
                else
                {
                    Unload(position);
                }
            }
        }

        private enum TaskType
        {
            Load,
            Unload
        }
    }
}