using UnityEngine;

namespace Core.Model.Space
{
    public readonly struct Square
    {
        public readonly int Size;
        public readonly int LeftX;
        public readonly int BottomY;

        public Square(int size, int leftX, int bottomY)
        {
            Size = size;
            LeftX = leftX;
            BottomY = bottomY;
        }

        public Square Shift(Position delta)
        {
            return new Square(Size, LeftX + delta.X, BottomY + delta.Y);
        }

        public Position Center()
        {
            var offset = Mathf.CeilToInt(Size / 2F) - 1;
            
            return new Position(LeftX + offset, BottomY + offset);
        }

        public Position BottomLeft()
        {
            return new Position(LeftX, BottomY);
        }

        public Position TopLeft()
        {
            return new Position(LeftX, BottomY + Size - 1);
        }

        public Position BottomRight()
        {
            return new Position(LeftX + Size - 1, BottomY);
        }

        public Position TopRight()
        {
            return new Position(LeftX + Size - 1, BottomY + Size - 1);
        }
    }
}