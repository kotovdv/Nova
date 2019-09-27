using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Core.Model.Space;

namespace Core.Configuration
{
    public class ConcurrentSpaceTileProvider : ISpaceTileProvider
    {
        private readonly int _threadsCount;
        private readonly SpaceTileProvider _spaceTileProvider;
        private readonly BlockingCollection<SpaceTile> _storage;

        public static ConcurrentSpaceTileProvider Construct(
            int threadsCount,
            GameConfiguration conf,
            SpaceTileProvider provider
        )
        {
            var tilesPerBiggestSide = conf.MaxZoom / conf.TileSize;
            var maxTiles = 4 * (tilesPerBiggestSide - 1);
            var storage = new BlockingCollection<SpaceTile>(maxTiles);

            var instance = new ConcurrentSpaceTileProvider(threadsCount, provider, storage);
            instance.Start();
            return instance;
        }

        private ConcurrentSpaceTileProvider(
            int threadsCount,
            SpaceTileProvider spaceTileProvider,
            BlockingCollection<SpaceTile> storage)
        {
            _storage = storage;
            _threadsCount = threadsCount;
            _spaceTileProvider = spaceTileProvider;
        }

        private void Start()
        {
            for (var i = 0; i < _threadsCount; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        GenerateTile();
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }

        public SpaceTile Take()
        {
            return _storage.Take();
        }

        private void GenerateTile()
        {
            var spaceTile = _spaceTileProvider.Take();
            _storage.Add(spaceTile);
        }
    }
}