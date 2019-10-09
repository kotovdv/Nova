using System;
using UnityEngine;

namespace Core.Model.Space
{
    [Serializable]
    public readonly struct Position : IComparable<Position>
    {
        public readonly int X;
        public readonly int Y;

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y);
        }

        public int At(Axis axis)
        {
            return axis == Axis.X ? X : Y;
        }

        public static Position operator -(Position p1)
        {
            return new Position(-p1.X, -p1.Y);
        }

        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Position operator -(Position p1, Position p2)
        {
            return new Position(p1.X - p2.X, p1.Y - p2.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Position other && Equals(ref other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }

        private bool Equals(ref Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public int CompareTo(Position other)
        {
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0) return xComparison;
            return Y.CompareTo(other.Y);
        }
    }
}