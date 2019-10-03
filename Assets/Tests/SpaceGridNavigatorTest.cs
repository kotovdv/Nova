using Core.Model.Space;
using Core.Model.Space.Grid;
using NUnit.Framework;

namespace Tests
{
    public class SpaceGridNavigatorTest
    {
        private const int TileSize = 100;
        private readonly SpaceGridNavigator _navigator = new SpaceGridNavigator(TileSize);

        [Test]
        public void ZeroPosition()
        {
            var result = _navigator.Find(new Position(0, 0));
            Assert.AreEqual(new Position(1, 1), result.TilePosition);
            Assert.AreEqual(new Position(0, 0), result.ElementPosition);
        }

        [Test]
        public void NonZeroPositivePositionInFirstTile()
        {
            var result = _navigator.Find(new Position(90, 90));
            Assert.AreEqual(new Position(1, 1), result.TilePosition);
            Assert.AreEqual(new Position(90, 90), result.ElementPosition);
        }

        [Test]
        public void PriorToTileSizePositivePosition()
        {
            var result1 = _navigator.Find(new Position(99, 99));
            Assert.AreEqual(new Position(1, 1), result1.TilePosition);
            Assert.AreEqual(new Position(99, 99), result1.ElementPosition);
        }

        [Test]
        public void TileSizePositivePosition()
        {
            var result = _navigator.Find(new Position(100, 100));
            Assert.AreEqual(new Position(2, 2), result.TilePosition);
            Assert.AreEqual(new Position(0, 0), result.ElementPosition);
        }
        [Test]
        public void AboveTileSizePositivePosition()
        {
            var result = _navigator.Find(new Position(101, 101));
            Assert.AreEqual(new Position(2, 2), result.TilePosition);
            Assert.AreEqual(new Position(1, 1), result.ElementPosition);
        }

        [Test]
        public void NonZeroNegativePositionInFirstTile()
        {
            var result = _navigator.Find(new Position(-10, -10));
            Assert.AreEqual(new Position(-1, -1), result.TilePosition);
            Assert.AreEqual(new Position(90, 90), result.ElementPosition);
        }

        [Test]
        public void PriorToTileSizeNegativePosition()
        {
            var result1 = _navigator.Find(new Position(-99, -99));
            Assert.AreEqual(new Position(-1, -1), result1.TilePosition);
            Assert.AreEqual(new Position(1, 1), result1.ElementPosition);
        }
        
        [Test]
        public void TileSizeNegativePosition()
        {
            var result1 = _navigator.Find(new Position(-100, -100));
            Assert.AreEqual(new Position(-1, -1), result1.TilePosition);
            Assert.AreEqual(new Position(0, 0), result1.ElementPosition);
        }
        
        [Test]
        public void AboveTileSizeNegativePosition()
        {
            var result = _navigator.Find(new Position(-101, -101));
            Assert.AreEqual(new Position(-2, -2), result.TilePosition);
            Assert.AreEqual(new Position(99, 99), result.ElementPosition);
        }
    }
}