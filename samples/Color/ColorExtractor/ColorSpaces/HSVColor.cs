using System;
using System.Collections.Generic;
using System.Text;

namespace ColorExtractor.ColorSpaces
{
    public struct HSVColor : IEquatable<HSVColor>
    {
        public HSVColor(int h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
        }

        public int H { get; set; }

        public float S { get; set; }

        public float V { get; set; }

        public bool Equals(HSVColor other)
        {
            return H == other.H &&
                   S == other.S &&
                   V == other.V;
        }
    }
}
