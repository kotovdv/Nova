using System;
using Newtonsoft.Json;

namespace Core.Model.Space
{
    [Serializable]
    public readonly struct Color
    {
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;

        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public UnityEngine.Color ToUnityEngineColor()
        {
            return new UnityEngine.Color(R / 255F, G / 255F, B / 255F);
        }

        public bool Equals(Color other)
        {
            return R == other.R && G == other.G && B == other.B;
        }

        public override bool Equals(object obj)
        {
            return obj is Color other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(R)}: {R}, {nameof(G)}: {G}, {nameof(B)}: {B}";
        }
    }
}