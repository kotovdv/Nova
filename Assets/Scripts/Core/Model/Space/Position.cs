using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Core.Model.Space
{
    [Serializable]
    [DataContract]
    public readonly struct Position
    {
        [DataMember]
        public readonly int X;

        [DataMember]
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
            return obj is Position other && Equals(other);
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

        private bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}