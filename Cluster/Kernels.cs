using System;

namespace ClusterLib
{
    public static class Kernels
    {
        public static double FlatKernel(double distance, double window)
        {
            if (distance < window)
            {
                return .5d;
            }
            return 0;
        }

        public static double GaussianKernel(double distance, double window)
        {
            // c1 and c2 are just caches for convience.
            double c1 = -.5 * distance * distance;
            c1 = Math.Pow(Math.E, c1);
            double c2 = (Math.PI * 2);
            c2 = Math.Pow(c2, window / 2);
            return c1 / c2;
        }
    }
}
