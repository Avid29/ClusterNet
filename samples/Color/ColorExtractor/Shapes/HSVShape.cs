using ClusterLib.Shapes;
using ColorExtractor.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColorExtractor.Shapes
{
    public struct HSVShape : IPoint<HSVColor>
    {
        public HSVColor Average(IEnumerable<HSVColor> items)
        {
            int sumH, count;
            sumH = count = 0;
            double sumS, sumV;
            sumS = sumV = 0;

            foreach (var item in items)
            {
                sumH += item.H;
                sumS += item.S;
                sumV += item.V;
                count++;
            }

            HSVColor color = new HSVColor()
            {
                H = (int)(sumH / count),
                S = (float)(sumS / count),
                V = (float)(sumV / count),
            };

            return color;
        }

        public double FindDistanceSquared(HSVColor it1, HSVColor it2)
        {
            int hueAngle1 = Math.Abs(it1.H - it2.H);
            int hueAngle2 = Math.Abs(it2.H - it1.H);
            float hueDiff = Math.Min(hueAngle1, hueAngle2) / 360;
            float satDiff = Math.Abs(it1.S - it2.S);
            float valDiff = Math.Abs(it1.V - it2.V);

            hueDiff = hueDiff * satDiff;
            hueDiff *= 2;
            return hueDiff * hueDiff + satDiff * satDiff + valDiff * valDiff;
        }

        public HSVColor WeightedAverage(IEnumerable<(HSVColor, double)> items)
        {
            double sumH, sumS, sumV;
            sumH = sumS = sumV = 0;
            double totalWeight = 0;
            foreach (var item in items)
            {
                sumH += item.Item1.H * item.Item2;
                sumS += item.Item1.S * item.Item2;
                sumV += item.Item1.V * item.Item2;
                totalWeight += item.Item2;
            }
            HSVColor color = new HSVColor()
            {
                H = (int)(sumH / totalWeight),
                S = (float)(sumS / totalWeight),
                V = (float)(sumV / totalWeight),
            };
            return color;
        }
    }
}
