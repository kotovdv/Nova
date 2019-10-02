namespace Core.Model.Space
{
    public enum Axis
    {
        X = 0,
        Y = 1
    }

    public static class AxisExtensions
    {
        public static Axis Opposite(this Axis axis)
        {
            return Axis.X == axis ? Axis.Y : Axis.X;
        }
    }
}