using System;

namespace ClusterLib.Kernels
{
    /// <summary>
    /// A Kernel with a gaussian cutoff.
    /// </summary>
    public class GaussianKernel : IKernel
    {
        private double _bandwidthSquared;

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianKernel"/> class.
        /// </summary>
        /// <param name="bandwidth">The bandwidth of the <see cref="GaussianKernel"/>.</param>
        public GaussianKernel(double bandwidth)
        {
            _bandwidthSquared = bandwidth * bandwidth;
        }

        /// <inheritdoc/>
        public double WeightDistance(double distanceSquared)
        {
            return Math.Pow(Math.E, -.5 * distanceSquared / _bandwidthSquared);
        }
    }
}
