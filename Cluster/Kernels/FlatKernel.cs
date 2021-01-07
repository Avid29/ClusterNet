using System;
using System.Collections.Generic;
using System.Text;

namespace ClusterLib.Kernels
{
    public class FlatKernel : IKernel
    {
        public double _windowSquared;

        public FlatKernel(double window)
        {
            _windowSquared = window * window;
        }

        public double WeightDistance(double distanceSquared)
        {
            return distanceSquared < _windowSquared ? 1 : 0;
        }
    }
}
