// Adam Dernis © 2021

using ClusterNet.Shapes;

namespace ClusterNet.Abstract
{
    /// <summary>
    /// The base class to represent a Cluster in an implemented algorithm.
    /// </summary>
    /// <typeparam name="T">The type of data in the cluster.</typeparam>
    /// <typeparam name="TShape">A shape to describe to provide comparison methods for <see cref="T"/>.</typeparam>
    public abstract class Cluster<T, TShape>
        where T : unmanaged
        where TShape : struct, IPoint<T>
    {
        /// <summary>
        /// Tracks the last calculated centroid.
        /// </summary>
        protected T? _centroid;

        /// <summary>
        /// Gets the center point of all points in the <see cref="Cluster{T, TShape}"/>, calculated with average.
        /// </summary>
        public T Centroid
        {
            get => _centroid ?? (T)(_centroid = CalculateCentroid());
        }

        /// <summary>
        /// Calculates the Centroid for the values in the cluster.
        /// </summary>
        /// <returns>The center point of all values in the cluster.</returns>
        protected abstract T CalculateCentroid();
    }
}
