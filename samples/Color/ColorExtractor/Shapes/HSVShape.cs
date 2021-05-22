using ClusterNet.Shapes;
using ColorExtractor.ColorSpaces;
using System;
using System.Runtime.CompilerServices;

namespace ColorExtractor.Shapes
{
    public struct HSVShape : IPoint<HSVColor, HSVProgress>
    {
        public HSVProgress AddToAverage(HSVProgress avgProgress, HSVColor item)
        {
            avgProgress.H += item.H;
            avgProgress.S += item.S;
            avgProgress.V += item.V;
            avgProgress.TotalWeight++;
            return avgProgress;
        }

        public HSVProgress AddToAverage(HSVProgress avgProgress, (HSVColor, double) item)
        {
            avgProgress.H += item.Item1.H * item.Item2;
            avgProgress.S += item.Item1.S * item.Item2;
            avgProgress.V += item.Item1.V * item.Item2;
            avgProgress.TotalWeight += item.Item2;
            return avgProgress;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AreEqual(HSVColor it1, HSVColor it2, double error = 0)
        {
            return FindDistanceSquared(it1, it2) <= 0;
        }

        public HSVColor Average(HSVColor[] items)
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

        public HSVColor FinalizeAverage(HSVProgress avgProgress)
        {
            return new HSVColor(
                (int)(avgProgress.H / avgProgress.TotalWeight),
                (float)(avgProgress.S / avgProgress.TotalWeight),
                (float)(avgProgress.V / avgProgress.TotalWeight));
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

        public HSVColor WeightedAverage((HSVColor, double)[] items)
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
