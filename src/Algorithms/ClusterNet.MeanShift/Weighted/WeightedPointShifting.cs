// Adam Dernis © 2021

using ClusterNet.Kernels;
using ClusterNet.Shapes;
using System;

// Warning disabled for tuple pointers.
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace ClusterNet.MeanShift
{
    /// <summary>
    /// A class containing the weighted point shift methods.
    /// </summary>
    internal static partial class PointShifting
    {
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
        public static unsafe (T, int) MeanShiftPoint<T, TShape, TKernel>(
            (T, int) cluster,
            (T, int)* points,
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
            (T, int) newCluster; // TODO: Unsafe.SkipInit if/when available.
            while (changed)
            {
                newCluster = Shift<T, TShape, TKernel>(cluster, points, pointCount, kernel, weightedSubPointList);
                changed = !shape.AreEqual(newCluster.Item1, cluster.Item1, ACCEPTED_ERROR);
                cluster = newCluster;
            }

            return cluster;
        }

        /// <summary>
        /// Shifts a cluster towards its convergence point.
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
        public static unsafe (T, int) Shift<T, TShape, TKernel>(
            (T, int) p,
            (T, int)* points,
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
                (T, int) point = points[i];
                double distance = shape.FindDistanceSquared(p.Item1, point.Item1);
                double weight = kernel.WeightDistance(distance);
                weightedSubPointList[i] = (point.Item1, weight * point.Item2);
            }

            return (shape.WeightedAverage(weightedSubPointList), p.Item2);
        }
    }
}
#pragma warning restore SA1009 // Closing parenthesis should be spaced correctly