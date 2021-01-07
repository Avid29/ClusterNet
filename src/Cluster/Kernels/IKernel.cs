namespace ClusterLib.Kernels
{
    /// <summary>
    /// An interface for a Kernel distribution.
    /// </summary>
    /// <remarks>
    /// Kernel requirements
    /// 1: \int K(u)du = 1
    /// 2: K(u) = K(|u|)
    /// 
    /// In other words:
    /// The kernel must be normalized
    /// The kernel must be symmetric
    /// </remarks>
    public interface IKernel
    {
        /// <summary>
        /// Gets the weighted relevence of a point at sqrt(<paramref name="distanceSquared"/>) away.
        /// </summary>
        /// <param name="distanceSquared">The distance^2 of the point to be weighted.</param>
        /// <returns>The weight of a point at sqrt(<paramref name="distanceSquared"/>) away.</returns>
        double WeightDistance(double distanceSquared);
    }
}
