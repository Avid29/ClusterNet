using ClusterNet.Kernels;
using ClusterNet.Shapes;
using System;

namespace ClusterNet.MeanShift
{
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
        /// <param name="weightedSubPointList">The array to shift in (passed into to save allocation)</param>
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
            TShape shape = default;


            // Shift the cluster until it does not shift.
            bool changed = true;
            T newCluster = default;
            while (changed)
            {
                newCluster = Shift<T, TShape, TKernel>(cluster, points, pointCount, kernel, weightedSubPointList);
                changed = shape.FindDistanceSquared(newCluster, cluster) != 0;
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
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
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
