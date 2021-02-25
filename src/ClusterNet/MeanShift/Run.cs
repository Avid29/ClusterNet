﻿using ClusterNet.Kernels;
using ClusterNet.Shapes;
using System;
using System.Threading.Tasks;

namespace ClusterNet.MeanShift
{
    /// MeanShift finds clusters by moving clusters towards a convergence point.
    /// This initial position of a cluster is a clone of a coresponding point. If clusters share a position they can be merged into one.
    /// 
    /// Mathematically, the convergence point can be found by graphing the distribution from each point.
    /// After summing these distributions, the nearest local maxima to a cluster's initial
    /// position is that cluster's convergence point.
    ///
    /// Clusters at 1, 4, 4.5 and 5. Overlayed
    /// 
    ///           *                             *    *    *
    ///         *   *                         *   **   **   *
    ///       *       *                     *    *  * *  *    *
    ///     *           *                 *    *    * *    *    *
    ///   *               *             *    *    *     *    *    *
    /// *                   *         *    *    *         *    *    *
    /// 0 - - - - 1 - - - - 2 - - - - 3 - - - - 4 - - - - 5 - - - - 6
    ///           ·                             ·    ·    ·
    /// 
    /// Clusters at 1, 4, 4.5 and 5. Summed  
    ///                                                
    ///                                              *  
    ///                                            *   *       
    ///                                          *       * 
    ///                                         *         *
    ///                                         *         *              
    ///                                        *           *            
    ///                                       *             *
    ///           *                          *               *
    ///         *   *                       *                 *
    ///       *       *                    *                   *
    ///     *           *                 *                     *  
    ///   *               *             *                         *  
    /// *                   *         *                             *  
    /// 0 - - - - 1 - - - - 2 - - - - 3 - - - - 4 - - - - 5 - - - - 6
    ///           ·                             ·    ·    ·
    /// 
    /// The clusters would be 1 and 4.5, because those are all the local maximas.
    /// 
    /// 
    /// Programmatically, these clusters are found by continually shifting each cluster towards their convergence point.
    /// Each shift is performed by finding the cluster's distance from each point then weighting its effect on the cluster.
    /// These weights are then used to find a weighted average, the result of each is the new cluster position.


    /// <summary>
    /// A class containing root implementations of MeanShift algorithms.
    /// </summary>
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
        /// <remarks>
        /// It is usually wise to use <see cref="Methods.ClusterAlgorithms.WeightedMeanShiftMultiThreaded{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/> instead.
        /// Weighted MeanShift greatly reduces computation time when dealing with duplicate points.
        /// </remarks>
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
            /// When running MeanShift Multi-threaded, each cluster can be shifted on a different thread.
            /// Each cluster is calculated seperate from the others, so this requires minimal changes.
            /// Once each cluster has converged, the same PostProcess combinations can be run.
            
            T[] clusters = PrePost.SetupClusters(points, initialClusters);

            fixed (T* p0 = points)
            {
                T* p = p0;
                int pointCount = points.Length;

                // Shift each cluster until it's at its convergence point.

                // TODO: Explicit use explicit thread count with Environment.ProcessorCount.
                // This will cut memory usage by reducing the number of weightedSubPointLists and reduce strain on the task scheduler.
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
