// Adam Dernis © 2021

namespace ClusterNet.Kernels
{
    /// <summary>
    /// A Kernel with a flat cutoff at its window size.
    /// </summary>
    public struct FlatKernel : IKernel
    {
        private double _windowSquared;
        private double _window;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlatKernel"/> struct.
        /// </summary>
        /// <param name="window">The window size of the Kernel.</param>
        public FlatKernel(double window)
        {
            // These will be set in WindowSize
            _window = 0;
            _windowSquared = 0;

            WindowSize = window;
        }

        /// <inheritdoc/>
        public double WindowSize
        {
            get => _window;
            set
            {
                _window = value;
                _windowSquared = value * value;
            }
        }

        /// <inheritdoc/>
        public double WeightDistance(double distanceSquared)
        {
            return distanceSquared < _windowSquared ? 1 : 0;
        }
    }
}
