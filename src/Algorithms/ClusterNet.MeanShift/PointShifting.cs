// Adam Dernis © 2021

using ClusterNet.Kernels;
using ClusterNet.Shapes;
using System;

namespace ClusterNet.MeanShift
{
    /// <summary>
    /// A class containing the point shift methods.
    /// </summary>
    internal static partial class PointShifting
    {
        private const double ACCEPTED_ERROR = 0.000005;

        /// <summary>
        /// Runs shift on a single cluster with the full readonly points.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="cluster">The cluster to MeanShift.</param>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="pointCount">The amount of points in the array.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="weightedSubPointList">The array to shift in (passed into to save allocation).</param>
        /// <returns>The <paramref name="cluster"/> point fully shifted via MeanShift.</returns>
        public static unsafe T MeanShiftPoint<T, TShape, TKernel>(
            T cluster,
            T* points,
            int pointCount,
            TKernel kernel,
            (T, double)[] weightedSubPointList)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            // TODO: Change weightedSubPointList to a Span.
            TShape shape = default;

            // Shift the cluster until it does not shift.
            bool changed = true;
            T newCluster; // TODO: Unsafe.SkipInit if/when available.
            while (changed)
            {
                newCluster = Shift<T, TShape, TKernel>(cluster, points, pointCount, kernel, weightedSubPointList);
                changed = !shape.AreEqual(newCluster, cluster, ACCEPTED_ERROR);
                cluster = newCluster;
            }

            return cluster;
        }

        /// <summary>
        /// Shifts a cluster towards its convergence point. If the cluster is at its convergence point it doesn't move.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="p">The cluster to shift.</param>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="pointCount">The number of points in the point list.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="weightedSubPointList">The arrays to use for weighted subpoints while shifting.</param>
        /// <returns>The new cluster from the cluster being shifted.</returns>
        public static unsafe T Shift<T, TShape, TKernel>(
            T p,
            T* points,
            int pointCount,
            TKernel kernel,
            (T, double)[] weightedSubPointList)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            TShape shape = default;

            // Create cluster based on distance of points from the current cluster's centroid
            for (int i = 0; i < pointCount; i++)
            {
                T point = points[i];
                double distance = shape.FindDistanceSquared(p, point);
                double weight = kernel.WeightDistance(distance);
                weightedSubPointList[i] = (point, weight);
            }

            return shape.WeightedAverage(weightedSubPointList);
        }
    }
}
