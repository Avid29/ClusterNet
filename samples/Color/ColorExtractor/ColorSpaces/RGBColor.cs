namespace ColorExtractor.ColorSpaces
{
    public struct RGBColor
    {
        public RGBColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }
    }
}
