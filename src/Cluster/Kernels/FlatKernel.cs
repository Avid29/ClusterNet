namespace ClusterLib.Kernels
{
    /// <summary>
    /// A Kernel with a flat cutoff at its window size.
    /// </summary>
    public class FlatKernel : IKernel
    {
        private double _windowSquared;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlatKernel"/> class.
        /// </summary>
        /// <param name="window">The window size of the Kernel.</param>
        public FlatKernel(double window)
        {
            _windowSquared = window * window;
        }

        /// <inheritdoc/>
        public double WeightDistance(double distanceSquared)
        {
            return distanceSquared < _windowSquared ? 1 : 0;
        }
    }
}
