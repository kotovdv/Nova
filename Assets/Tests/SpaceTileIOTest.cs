using System.IO;
using Core.Model.Space;
using Core.Model.Space.Grid.IO;
using Core.Model.Space.Tiles;
using NUnit.Framework;

namespace Tests
{
    public class SpaceTileIOTest
    {
        private readonly SpaceTileIO _spaceTileIo = new SpaceTileIO(Path.GetTempPath());
        private readonly SpaceTileFactory _factory = new SpaceTileFactory(100, 10_000, 0, 100);

        [Test]
        public void SerializationAndDeserialization()
        {
            var expected = _factory.CreateTile();

            _spaceTileIo.Write(new Position(0, 0), ref expected);

            var actual = _spaceTileIo.Read(new Position(0, 0));

            for (var x = 0; x < 100; x++)
            for (var y = 0; y < 100; y++)
            {
                Assert.IsTrue(actual.GetValue(x,y).HasValue);
                Assert.AreEqual(actual.GetValue(x, y), expected.GetValue(x, y));
            }
        }
    }
}