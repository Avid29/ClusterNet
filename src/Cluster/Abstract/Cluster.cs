using ClusterLib.Shapes;

namespace ClusterLib.Abstract
{
    /// <summary>
    /// The base class to represent a Cluster in each implemented algorithm.
    /// </summary>
    /// <typeparam name="T">The type of data in the cluster.</typeparam>
    /// <typeparam name="TShape">A shape to describe to provide comparison methods for <see cref="T"/>.</typeparam>
    public abstract class Cluster<T, TShape>
        where T : unmanaged
        where TShape : struct, IPoint<T>
    {
        protected T? _centroid;

        /// <summary>
        /// The center point of all points in the <see cref="Cluster{T, TShape}"/>, calculated with average.
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
