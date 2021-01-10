using System;
using System.Runtime.CompilerServices;

namespace ClusterLib.Kernels
{
    /// <summary>
    /// A Kernel with a gaussian cutoff.
    /// </summary>
    public struct GaussianKernel : IKernel
    {
        private double _bandwidthSquared;

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianKernel"/> class.
        /// </summary>
        /// <param name="bandwidth">The bandwidth of the <see cref="GaussianKernel"/>.</param>
        public GaussianKernel(double bandwidth)
        {
            // * -2, to precompute the * -.5 in WeightDistance
            _bandwidthSquared = bandwidth * bandwidth * -2;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double WeightDistance(double distanceSquared)
        {
            //return Math.Pow(Math.E, -.5 * distanceSquared / _bandwidthSquared);
            return Math.Exp(distanceSquared / _bandwidthSquared);
        }
    }
}
