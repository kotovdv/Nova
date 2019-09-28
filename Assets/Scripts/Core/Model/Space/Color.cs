namespace Core.Model.Space
{
    public readonly struct Color
    {
        private readonly byte _r;
        private readonly byte _g;
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
    }
}