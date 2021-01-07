using System;

namespace ClusterLib.Kernels
{
    public class GaussianKernel : IKernel
    {
        public double _bandwidthSquared;

        public GaussianKernel(double bandwidth)
        {
            _bandwidthSquared = bandwidth * bandwidth;
        }

        public double WeightDistance(double distanceSquared)
        {
            return Math.Pow(Math.E, -.5 * distanceSquared / _bandwidthSquared);
        }
    }
}
