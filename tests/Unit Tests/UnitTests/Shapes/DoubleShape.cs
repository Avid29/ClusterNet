using ClusterLib.Shapes;
using System;
using System.Collections.Generic;

namespace UnitTests.Shapes
{
    public struct DoubleShape : IPoint<double>
    {
        public double Average(IEnumerable<double> items)
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

        public double Divide(double it, double count)
        {
            it /= count;
            return it;
        }

        public double FindDistanceSquared(double it1, double it2)
        {
            return Math.Abs(it1 - it2);
        }

        public double WeightedAverage(IEnumerable<(double, double)> items)
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
