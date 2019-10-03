using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Core.Model.Space.Grid.IO;
using Core.Util;

namespace Core.Model.Space.Tiles
{
    public class SpaceGridTileCache
    {
        private readonly Synchronizer<Position> _synchronizer = new Synchronizer<Position>();

        private readonly int _threadsCount;
        private readonly SpaceTileIO _tileIO;
        private readonly SpaceTileFactory _tileFactory;

        private long _taskId;
        
        private readonly ConcurrentStack<Task> _tasks = new ConcurrentStack<Task>();
        private readonly ConcurrentDictionary<Position, long> _tileLastTask = new ConcurrentDictionary<Position, long>();
        private readonly ConcurrentDictionary<Position, SpaceTile> _tiles = new ConcurrentDictionary<Position, SpaceTile>();
        private readonly ConcurrentDictionary<Position, bool?> _tilesLoadStatuses = new ConcurrentDictionary<Position, bool?>();

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
                var thread = new Thread(ConsumeTaskLoop) {Name = "TilesCacheThread_" + i};
                thread.Start();
            }
        }

        public SpaceTile Get(Position position)
        {
            lock (_synchronizer[position])
            {
                if (_tiles.TryGetValue(position, out var tile)) return tile;
                Load(position);

                return _tiles[position];
            }
        }

        public void LoadAsync(Position position)
        {
            AddTask(position, TaskType.Load);
        }

        public void UnloadAsync(Position position)
        {
            AddTask(position, TaskType.Unload);
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

        private void Unload(Position position)
        {
            lock (_synchronizer[position])
            {
                var isLoaded = _tilesLoadStatuses.GetOrDefault(position, null);

                if (!isLoaded.HasValue || !isLoaded.Value) return;

                var tile = _tiles[position];
                _tileIO.Write(position, ref tile);

                _tiles.TryRemove(position, out _);
                _tilesLoadStatuses[position] = false;
            }
        }

        private void ConsumeTaskLoop()
        {
            while (true) ConsumeTask();
        }

        private void ConsumeTask()
        {
            if (!_tasks.TryPop(out var task))
            {
                Thread.Sleep(ThreadLocalRandom.Current().Next(15, 31));
                return;
            }

            var position = task.Position;

            lock (_synchronizer[position])
            {
                if (task.Id <= _tileLastTask.GetOrDefault(position, -1)) return;

                if (task.Type == TaskType.Load)
                {
                    Load(position);
                }
                else
                {
                    Unload(position);
                }

                _tileLastTask[position] = task.Id;
            }
        }

        private void AddTask(Position position, TaskType taskType)
        {
            _tasks.Push(new Task(Interlocked.Increment(ref _taskId), position, taskType));
        }

        private enum TaskType
        {
            Load,
            Unload
        }

        private readonly struct Task
        {
            public readonly long Id;
            public readonly Position Position;
            public readonly TaskType Type;

            public Task(long id, Position position, TaskType type)
            {
                Position = position;
                Type = type;
                Id = id;
            }
        }
    }
}