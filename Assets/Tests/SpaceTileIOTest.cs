using System.IO;
using Core.Model.Space;
using Core.Model.Space.Grid;
using Core.Model.Space.Grid.Storage;
using NUnit.Framework;

namespace Tests
{
    public class SpaceTileIOTest
    {
        private readonly Planet?[][] _planets =
        {
            new Planet?[] {new Planet(35, new Color(1, 1, 1))},
            new Planet?[] {new Planet(100, new Color(10, 10, 10))}
        };

        private readonly SpaceTileIO _spaceTileIo = new SpaceTileIO(Path.GetTempPath());

        [Test]
        public void SerializationAndDeserialization()
        {
            var expected = new SpaceTile(_planets, new Position[] { });

            _spaceTileIo.Write(new Position(0, 0), ref expected);

            var actual = _spaceTileIo.Read(new Position(0, 0));

            Assert.IsTrue(actual[0, 0].Equals(expected[0, 0]));
            Assert.IsTrue(actual[1, 0].Equals(expected[1, 0]));
        }
    }
}