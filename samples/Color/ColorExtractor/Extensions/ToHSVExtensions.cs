using System;

namespace ColorExtractor.ColorSpaces
{
    public static class ToHSVExtensions
    {
        private enum RGBChannel
        { R, G, B }

        public static HSVColor AsHsv(this RGBColor color)
        {
            return new HSVColor()
            {
                H = color.GetHue(),
                S = color.GetSaturation(),
                V = color.GetValue(),
            };
        }

        public static int GetHue(this RGBColor color)
        {
            double delta = GetDelta(color);

            if (delta == 0)
                return 0;

            double r = (double)color.R / 255;
            double g = (double)color.G / 255;
            double b = (double)color.B / 255;

            switch (GetCMaxChannel(color))
            {
                case RGBChannel.R:
                    return (int)(60 * (((g - b) / delta) % 6));
                case RGBChannel.G:
                    return (int)(60 * (((b - r) / delta) + 2));
                case RGBChannel.B:
                    return (int)(60 * (((r - g) / delta) + 4));
            }

            return 0; // Not possible
        }
        public static float GetSaturation(this RGBColor color)
        {
            float value = color.GetValue();

            if (value == 0)
            {
                return 0;
            }

            float delta = GetDelta(color);
            return delta / value;
        }
        public static float GetValue(this RGBColor color)
        {
            return (float)GetRawCMax(color) / 255;
        }
        internal static float GetCMin(this RGBColor color)
        {
            return (float)GetRawCMin(color) / 255;
        }
        private static float GetDelta(this RGBColor color)
        {
            return GetValue(color) - GetCMin(color);
        }
        private static int GetRawCMax(this RGBColor color)
        {
            int max = Math.Max(color.R, color.G);
            return Math.Max(max, color.B);
        }
        private static int GetRawCMin(this RGBColor color)
        {
            int min = Math.Min(color.R, color.G);
            return Math.Min(min, color.B);
        }
        private static RGBChannel GetCMaxChannel(this RGBColor color)
        {
            int maxValue = GetRawCMax(color);
            if (maxValue == color.R)
                return RGBChannel.R;
            if (maxValue == color.G)
                return RGBChannel.G;
            if (maxValue == color.B)
                return RGBChannel.B;

            return RGBChannel.R;
        }
    }
}
