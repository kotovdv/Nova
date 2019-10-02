using System.IO;
using System.Runtime.Serialization;
using Core.Model.Space.Tiles;
using Newtonsoft.Json;

namespace Core.Model.Space.Grid.IO
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

            var serializeObject = JsonConvert.SerializeObject(tile);

            File.WriteAllText(filePath, serializeObject);
        }

        public SpaceTile Read(Position tilePosition)
        {
            var filePath = ToFilePath(tilePosition);

            var fileExists = File.Exists(filePath);
            if (!fileExists) throw new FileNotFoundException(filePath);

            var text = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<SpaceTile>(text);
        }

        private string ToFilePath(Position tilePosition)
        {
            return _directory + tilePosition.X + "_" + tilePosition.Y;
        }
    }
}