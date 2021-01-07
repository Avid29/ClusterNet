using System;

namespace ClusterLib
{
    public static class Kernels
    {
        public static double FlatKernel(double distance, double kernelBandwidth)
        {
            return distance < kernelBandwidth ? 1 : 0;
        }

        public static double GaussianKernel(double distanceSquared, double kernelBandwidthSquared)
        {
            return Math.Pow(Math.E, -.5 * distanceSquared / kernelBandwidthSquared);
        }
    }
}
