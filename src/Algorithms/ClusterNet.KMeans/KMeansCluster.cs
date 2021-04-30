using ClusterNet.Abstract;
using ClusterNet.Shapes;
using System.Collections.Generic;

namespace ClusterNet.KMeans
{
    /// <summary>
    /// A <see cref="Cluster{T, TShape}"/> implementation for the KMeans algorithm.
    /// </summary>
    /// <typeparam name="T">The type of data in the cluster.</typeparam>
    /// <typeparam name="TShape">A shape to describe to provide comparison methods for <see cref="T"/>.</typeparam>
    public class KMeansCluster<T, TShape> : Cluster<T, TShape>
        where T : unmanaged
        where TShape : struct, IPoint<T>
    {
        private List<T> _subPointSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="KMeansCluster{T, TShape}"/> class.
        /// </summary>
        public KMeansCluster()
        {
            _subPointSet = new List<T>();
        }

        /// <summary>
        /// Gets the amount of elements in the cluster.
        /// </summary>
        public int Count => _subPointSet.Count;

        /// <summary>
        /// Gets or sets an element at an index in the sub point list.
        /// </summary>
        /// <param name="i">The index of the element to get or set.</param>
        /// <returns>The element at index <paramref name="i"/> in the sub point list.</returns>
        public T this[int i]
        {
            get => _subPointSet[i];
            set => _subPointSet[i] = value;
        }

        /// <summary>
        /// Adds a point to the <see cref="KMeansCluster{T, TShape}"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            _subPointSet.Add(item);
            _centroid = null;
        }

        /// <summary>
        /// Removes the point at <paramref name="index"/> from the sub point list.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        /// <returns>The removed item.</returns>
        public T RemoveAt(int index)
        {
            T removed = _subPointSet[index];
            _subPointSet.RemoveAt(index);
            return removed;
        }

        /// <summary>
        /// Gets the element in the cluster closest to the Centroid.
        /// </summary>
        /// <returns>The element in the cluster nearest the Centroid.</returns>
        public T GetNearestToCenter()
        {
            TShape shape = default;

            // Track nearest seen value and its index.
            double minimumDistance = double.PositiveInfinity;
            int nearestPointIndex = -1;

            for (int i = 0; i < _subPointSet.Count; i++)
            {
                T p = _subPointSet[i];
                double distance = shape.FindDistanceSquared(p, Centroid);

                // Update tracking variables
                if (minimumDistance > distance)
                {
                    minimumDistance = distance;
                    nearestPointIndex = i;
                }
            }

            // return the value at the index with the largest value found
            return _subPointSet[nearestPointIndex];
        }

        /// <inheritdoc/>
        protected override T CalculateCentroid()
        {
            TShape shape = default;
            return shape.Average(_subPointSet.ToArray());
        }
    }
}
