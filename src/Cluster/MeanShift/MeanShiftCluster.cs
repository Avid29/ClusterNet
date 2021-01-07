using ClusterLib.Abstract;
using ClusterLib.Shapes;
using System.Collections.Generic;

namespace ClusterLib
{
    /// <summary>
    /// A <see cref="Cluster{T, TShape}"/> implementation for the MeanShift algorithm.
    /// </summary>
    /// <typeparam name="T">The type of data in the cluster.</typeparam>
    /// <typeparam name="TShape">A shape to describe to provide comparison methods for <see cref="T"/>.</typeparam>
    public class MeanShiftCluster<T, TShape> : Cluster<T, TShape>
        where T : unmanaged
        where TShape : struct, IPoint<T>
    {
        private List<(T, double)> _weightedSubPointList;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeanShiftCluster{T, TShape}"/> class.
        /// </summary>
        public MeanShiftCluster()
            : base()
        {
            _centroid = null;
            _weightedSubPointList = new List<(T, double)>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeanShiftCluster{T, TShape}"/> class.
        /// </summary>
        /// <param name="point">An initial point in the cluster.</param>
        public MeanShiftCluster(T point)
        {
            _centroid = point;
            _weightedSubPointList = new List<(T, double)>();
            _weightedSubPointList.Add((point, 1));
        }

        /// <summary>
        /// Adds a point to the <see cref="MeanShiftCluster{T, TShape}"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="weight">The weight of the item.</param>
        internal void Add(T p, double weight = 1)
        {
            _weightedSubPointList.Add((p, weight));
            _centroid = null;
        }

        /// <summary>
        /// Gets the element in the cluster closest to the Centroid.
        /// </summary>
        /// <returns>The element in the cluster nearest the Centroid.</returns>
        public T GetNearestToCenter()
        {
            TShape shape = default;

            // Track furthest seen value and its index.
            double minimumDistance = double.PositiveInfinity;
            int nearestPointIndex = -1;

            for (int i = 0; i < _weightedSubPointList.Count; i++)
            {
                T p = _weightedSubPointList[i].Item1;
                double distance = shape.FindDistanceSquared(p, Centroid);

                // Update tracking variables
                if (minimumDistance > distance)
                {
                    minimumDistance = distance;
                    nearestPointIndex = i;
                }
            }

            // return the value at the index with the largest value found
            return _weightedSubPointList[nearestPointIndex].Item1;
        }

        /// <inheritdoc/>
        protected override T CalculateCentroid()
        {
            TShape shape = default;
            return shape.WeightedAverage(_weightedSubPointList);
        }
    }
}
