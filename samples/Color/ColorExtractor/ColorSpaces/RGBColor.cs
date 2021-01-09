namespace ColorExtractor.ColorSpaces
{
    public struct RGBColor
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
    }
}
