using ClusterLib;
using ClusterLib.Shapes;
using System;

namespace UnitTests.Mean1D
{
    public struct FlatDoubleShape : IPoint<double>
    {
        public double Sum(double it1, double it2, double weight = 1)
        {
            it1 += it2 * weight;
            return it1;
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

        public double WeightDistance(double distance, double window) =>
            Kernels.FlatKernel(distance, window);
    }
}
