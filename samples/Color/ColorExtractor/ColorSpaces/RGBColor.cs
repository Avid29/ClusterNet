using System;

namespace ColorExtractor.ColorSpaces
{
    public struct RGBColor : IEquatable<RGBColor>
    {
        public RGBColor(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public float R { get; set; }

        public float G { get; set; }

        public float B { get; set; }

        public bool Equals(RGBColor other)
        {
            return R == other.R &&
                   G == other.G &&
                   B == other.B;
        }
    }
}
