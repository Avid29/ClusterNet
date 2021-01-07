using ClusterLib.Shapes;
using ColorExtractor.ColorSpaces;
using System.Collections.Generic;

namespace ColorExtractor.Shapes
{
    public struct RGBShape : IPoint<RGBColor>
    {
        public RGBColor Average(IEnumerable<RGBColor> items)
        {
            int sumR, sumG, sumB, count;
            sumR = sumG = sumB = count = 0;
            foreach(var item in items)
            {
                sumR += item.R;
                sumG += item.G;
                sumB += item.B;
                count++;
            }

            RGBColor color = new RGBColor()
            {
                R = (byte)(sumR / count),
                G = (byte)(sumG / count),
                B = (byte)(sumB / count),
            };

            return color;
        }

        public double FindDistanceSquared(RGBColor it1, RGBColor it2)
        {
            int r = it1.R - it2.R;
            int g = it1.G - it2.G;
            int b = it1.B - it2.B;

            return r * r + g * g + b * b;
        }

        public RGBColor WeightedAverage(IEnumerable<(RGBColor, double)> items)
        {
            double sumR, sumG, sumB;
            sumR = sumG = sumB = 0;
            double totalWeight = 0;
            foreach (var item in items)
            {
                sumR += item.Item1.R * (float)item.Item2;
                sumG += item.Item1.G * (float)item.Item2;
                sumB += item.Item1.B * (float)item.Item2;
                totalWeight += item.Item2;
            }
            RGBColor color = new RGBColor()
            {
                R = (byte)(sumR / (float)totalWeight),
                G = (byte)(sumG / (float)totalWeight),
                B = (byte)(sumB / (float)totalWeight),
            };
            return color;
        }
    }
}
