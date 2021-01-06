using System;

namespace ClusterLib
{
    public static class Kernels
    {
        public static double FlatKernel(double distance, double kernelBandwidth)
        {
            if (distance < kernelBandwidth)
            {
                return .5d;
            }
            return 0;
        }

        public static double GaussianKernel(double distance, double kernelBandwidth)
        {
            // c1 and c2 are just caches for convience.
            double c1 = -.5 * distance * distance;
            c1 = Math.Pow(Math.E, c1);;
            return c1 / kernelBandwidth;
        }
    }
}
