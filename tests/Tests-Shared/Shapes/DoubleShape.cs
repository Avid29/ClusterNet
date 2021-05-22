using ClusterNet.Shapes;
using System;

namespace Tests.Shapes
{
    public struct DoubleShape : IPoint<double, (double, double)>
    {
        public (double, double) AddToAverage((double, double) avgProgress, double item)
        {
            avgProgress.Item1 += item;
            avgProgress.Item2++;
            return avgProgress;
        }

        public (double, double) AddToAverage((double, double) avgProgress, (double, double) item)
        {
            avgProgress.Item1 += item.Item1 * item.Item2;
            avgProgress.Item2 += item.Item2;
            return avgProgress;
        }

        public bool AreEqual(double it1, double it2)
        {
            return it1 == it2;
        }

        public bool AreEqual(double it1, double it2, double error = 0)
        {
            return FindDistanceSquared(it1, it2) <= error;
        }

        public double Average(double[] items)
        {
            double sum = 0;
            int count = 0;
            foreach (var item in items)
            {
                sum += item;
                count++;
            }
            return sum /= count;
        }

        public double FinalizeAverage((double, double) avgProgress)
        {
            return avgProgress.Item1 / avgProgress.Item2;
        }

        public double FindDistanceSquared(double it1, double it2)
        {
            return Math.Abs(it1 - it2);
        }

        public double WeightedAverage((double, double)[] items)
        {
            double sum = 0;
            double totalWeight = 0;
            foreach (var item in items)
            {
                sum += item.Item1 * item.Item2;
                totalWeight += item.Item2;
            }
            return sum / totalWeight;
        }
    }
}
