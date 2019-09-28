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
    }
}