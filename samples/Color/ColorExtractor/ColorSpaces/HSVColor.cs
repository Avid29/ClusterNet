using System;
using System.Collections.Generic;
using System.Text;

namespace ColorExtractor.ColorSpaces
{
    public struct HSVColor
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
    }
}
