using ClusterLib;
using ClusterLib.Shapes;
using System;
using System.Collections.Generic;

namespace UnitTests.Shapes
{
    public struct FlatDoubleShape : IPoint<double>
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

        public double FindDistance(double it1, double it2)
        {
            return Math.Abs(it1 - it2);
        }

        public double Sum(double it1, double it2)
        {
            return it1 + it2;
        }

        public double WeightDistance(double distance, double kernelBandwidth) =>
            Kernels.FlatKernel(distance, kernelBandwidth);

        public double WeightedAverage(IEnumerable<(double, double)> items)
        {
            throw new NotImplementedException();
        }
    }
}
