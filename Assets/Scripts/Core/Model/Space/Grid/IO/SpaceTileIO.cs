using System.IO;
using System.Runtime.Serialization;

namespace Core.Model.Space.Grid.Storage
{
    public class SpaceTileIO
    {
        private readonly string _directory;

        private readonly DataContractSerializer _serializer = new DataContractSerializer(typeof(SpaceTile));

        public SpaceTileIO(string folder)
        {
            _directory = folder + "/tiles/";
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
        }

        public void Write(Position tilePosition, ref SpaceTile tile)
        {
            var filePath = ToFilePath(tilePosition);

            var fileExists = File.Exists(filePath);
            if (fileExists) File.Delete(filePath);

            using (var stream = File.Create(filePath))
            {
                _serializer.WriteObject(stream, tile);
            }
        }

        public SpaceTile Read(Position tilePosition)
        {
            var filePath = ToFilePath(tilePosition);

            var fileExists = File.Exists(filePath);
            if (!fileExists) throw new FileNotFoundException(filePath);

            using (var stream = File.OpenRead(filePath))
            {
                return (SpaceTile) _serializer.ReadObject(stream);
            }
        }

        private string ToFilePath(Position tilePosition)
        {
            return _directory + tilePosition.X + "_" + tilePosition.Y;
        }
    }
}