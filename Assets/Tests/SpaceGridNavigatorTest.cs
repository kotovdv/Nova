using Core.Model.Space;
using Core.Model.Space.Grid;
using NUnit.Framework;

namespace Tests
{
    public class SpaceGridNavigatorTest
    {
        private readonly SpaceGridNavigator _singleSizeGrizNavigator = new SpaceGridNavigator(1);
        private readonly SpaceGridNavigator _fourSizeGridNavigator = new SpaceGridNavigator(4);
        private readonly SpaceGridNavigator _fiveSizeGridNavigator = new SpaceGridNavigator(5);

        [Test]
        public void SingleSizeGridNavigatorZeroZero()
        {
            var zeroPos = new Position(0, 0);

            var zeroPosTile = _singleSizeGrizNavigator.Find(zeroPos);
            Assert.AreEqual(zeroPos, zeroPosTile.TilePosition);
            Assert.AreEqual(zeroPos, zeroPosTile.ElementPosition);
        }

        [Test]
        public void SingleSizeGridNavigatorTenTen()
        {
            var zeroPos = new Position(0, 0);
            var tenPos = new Position(10, 10);

            var tenPosTile = _singleSizeGrizNavigator.Find(tenPos);
            Assert.AreEqual(tenPos, tenPosTile.TilePosition);
            Assert.AreEqual(zeroPos, tenPosTile.ElementPosition);
        }

        [Test]
        public void SingleSizeGridNavigatorMinusTenMinusTen()
        {
            var zeroPos = new Position(0, 0);
            var minusTenPos = new Position(-10, -10);

            var minusTenPosTile = _singleSizeGrizNavigator.Find(minusTenPos);
            Assert.AreEqual(minusTenPos, minusTenPosTile.TilePosition);
            Assert.AreEqual(zeroPos, minusTenPosTile.ElementPosition);
        }

        [Test]
        public void FourSizeGridNavigatorZeroZero()
        {
            var zeroPosition = _fourSizeGridNavigator.Find(new Position(0, 0));

            Assert.AreEqual(new Position(0, 0), zeroPosition.TilePosition);
            Assert.AreEqual(new Position(2, 2), zeroPosition.ElementPosition);
        }

        [Test]
        public void FourSizeGridNavigatorTwoTwo()
        {
            var plusHalfTilePosition = _fourSizeGridNavigator.Find(new Position(2, 2));

            Assert.AreEqual(new Position(1, 1), plusHalfTilePosition.TilePosition);
            Assert.AreEqual(new Position(0, 0), plusHalfTilePosition.ElementPosition);
        }

        [Test]
        public void FourSizeGridNavigatorMinusTwoMinusTwo()
        {
            var minusHalfTilePosition = _fourSizeGridNavigator.Find(new Position(-2, -2));

            Assert.AreEqual(new Position(0, 0), minusHalfTilePosition.TilePosition);
            Assert.AreEqual(new Position(0, 0), minusHalfTilePosition.ElementPosition);
        }

        [Test]
        public void FourSizeGridNavigatorThreeThree()
        {
            var plusMoreThanHalfTilePosition = _fourSizeGridNavigator.Find(new Position(3, 3));

            Assert.AreEqual(new Position(1, 1), plusMoreThanHalfTilePosition.TilePosition);
            Assert.AreEqual(new Position(1, 1), plusMoreThanHalfTilePosition.ElementPosition);
        }
        
        [Test]
        public void FourSizeGridNavigatorMinusThreeMinusThree()
        {
            var minusMoreThanHalfTilePosition = _fourSizeGridNavigator.Find(new Position(-3, -3));

            Assert.AreEqual(new Position(-1, -1), minusMoreThanHalfTilePosition.TilePosition);
            Assert.AreEqual(new Position(3, 3), minusMoreThanHalfTilePosition.ElementPosition);
        }
        
        [Test]
        public void FiveSizeGridNavigatorZeroZero()
        {
            var zeroPosition = _fiveSizeGridNavigator.Find(new Position(0, 0));

            Assert.AreEqual(new Position(0, 0), zeroPosition.TilePosition);
            Assert.AreEqual(new Position(2, 2), zeroPosition.ElementPosition);
        }
        
        [Test]
        public void FiveSizeGridNavigatorTwoTwo()
        {
            var plusHalfTilePosition = _fiveSizeGridNavigator.Find(new Position(2, 2));

            Assert.AreEqual(new Position(0, 0), plusHalfTilePosition.TilePosition);
            Assert.AreEqual(new Position(4, 4), plusHalfTilePosition.ElementPosition);
        }
        
        [Test]
        public void FiveSizeGridNavigatorMinusTwoMinusTwo()
        {
            var minusHalfTilePosition = _fiveSizeGridNavigator.Find(new Position(-2, -2));

            Assert.AreEqual(new Position(0, 0), minusHalfTilePosition.TilePosition);
            Assert.AreEqual(new Position(0, 0), minusHalfTilePosition.ElementPosition);
        }
        
        [Test]
        public void FiveSizeGridNavigatorFourFour()
        {
            var plusMoreThanHalfTilePosition = _fiveSizeGridNavigator.Find(new Position(4, 4));

            Assert.AreEqual(new Position(1, 1), plusMoreThanHalfTilePosition.TilePosition);
            Assert.AreEqual(new Position(1, 1), plusMoreThanHalfTilePosition.ElementPosition);
        }

        [Test]
        public void FiveSizeGridNavigatorMinusFourMinusFour()
        {
            var minusMoreThanHalfTilePosition = _fiveSizeGridNavigator.Find(new Position(-4, -4));

            Assert.AreEqual(new Position(-1, -1), minusMoreThanHalfTilePosition.TilePosition);
            Assert.AreEqual(new Position(3, 3), minusMoreThanHalfTilePosition.ElementPosition);
        }
    }
}