using ClusterNet.Kernels;
using ClusterNet.Shapes;
using System;
using System.Threading.Tasks;

namespace ClusterNet.MeanShift
{
    internal class Run
    {
        /// <summary>
        /// Runs MeanShift cluster on a list of <typeparamref name="T"/> points.
        /// </summary>
        /// <remarks>
        /// It is usually wise to use <see cref="Methods.ClusterAlgorithms.WeightedMeanShift{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/> instead.
        /// Weighted MeanShift greatly reduces computation time when dealing with duplicate points.
        /// </remarks>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="initialClusters">How many of the points to shift into place. 0 means one for each point.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static unsafe (T, int)[] MeanShift<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            T[] clusters = PrePost.SetupClusters(points, initialClusters);

            // Define this here, and reuse it on every iteration of Shift.
            (T, double)[] weightedSubPointList = new (T, double)[points.Length];

            fixed (T* p = points)
            {
                for (int i = 0; i < clusters.Length; i++)
                {
                    T cluster = clusters[i];
                    clusters[i] = PointShifting.MeanShiftPoint<T, TShape, TKernel>(cluster, p, points.Length, kernel, weightedSubPointList);
                }
            }

            return PrePost.PostProcess<T, TShape>(clusters);
        }

        /// <summary>
        /// Runs MeanShift cluster on a list of <typeparamref name="T"/> points. Runs in parallel.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="initialClusters">How many of the points to shift into place. 0 means one for each point.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static unsafe (T, int)[] MeanShiftMultiThreaded<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            T[] clusters = PrePost.SetupClusters(points, initialClusters);

            fixed (T* p0 = points)
            {
                T* p = p0;
                int pointCount = points.Length;

                // Shift each cluster until it's at its convergence point.
                Parallel.For(0, clusters.Length, (i, state) =>
                {
                    // Define this here, and reuse it on every iteration of Shift on this thread.
                    (T, double)[] weightedSubPointList = new (T, double)[pointCount];
                    T cluster = clusters[i];
                    clusters[i] = PointShifting.MeanShiftPoint<T, TShape, TKernel>(cluster, p, pointCount, kernel, weightedSubPointList);
                });
            }

            return PrePost.PostProcess<T, TShape>(clusters);
        }
    }
}
