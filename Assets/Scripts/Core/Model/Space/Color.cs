using System;
using System.Runtime.Serialization;

namespace Core.Model.Space
{
    [Serializable]
    [DataContract]
    public readonly struct Color
    {
        [DataMember]
        private readonly byte _r;

        [DataMember]
        private readonly byte _g;

        [DataMember]
        private readonly byte _b;

        public Color(byte r, byte g, byte b)
        {
            _r = r;
            _g = g;
            _b = b;
        }

        public UnityEngine.Color ToUnityEngineColor()
        {
            return new UnityEngine.Color(_r / 255F, _g / 255F, _b / 255F);
        }

        public bool Equals(Color other)
        {
            return _r == other._r && _g == other._g && _b == other._b;
        }

        public override bool Equals(object obj)
        {
            return obj is Color other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _r.GetHashCode();
                hashCode = (hashCode * 397) ^ _g.GetHashCode();
                hashCode = (hashCode * 397) ^ _b.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(_r)}: {_r}, {nameof(_g)}: {_g}, {nameof(_b)}: {_b}";
        }
    }
}