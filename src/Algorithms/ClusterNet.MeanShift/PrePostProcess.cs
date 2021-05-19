// Adam Dernis © 2021

using ClusterNet.Helpers;
using ClusterNet.Kernels;
using ClusterNet.Shapes;
using Microsoft.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusterNet.MeanShift
{
    /// <summary>
    /// A class containing pre and post process methods for the MeanShift method.
    /// </summary>
    internal static class PrePostProcess
    {
        /// <summary>
        /// Sets up the cluster array to be shifted.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <param name="points">The full list of points to apply MeanShift with.</param>
        /// <param name="initialClusters">The amount of points to use as clusters. 0 for all.</param>
        /// <returns>An array of <typeparamref name="T"/>s to be clustered.</returns>
        public static T[] SetupClusters<T>(
            ReadOnlySpan<T> points,
            int initialClusters)
            where T : unmanaged, IEquatable<T>
        {
            int n;
            if (initialClusters == 0)
            {
                initialClusters = points.Length;
                n = 1;
            }
            else
            {
                n = points.Length / initialClusters;
            }

            // N will be 0 if initialClusters is greater than the point count.
            // N can't be 0
            if (n == 0)
            {
                n = 1;
                initialClusters = points.Length;
            }

            // Create a cluster for each point.
            T[] clusters = new T[initialClusters];
            for (int i = 0; i < clusters.Length; i++)
            {
                clusters[i] = points[i * n];
            }

            return clusters;
        }

        /// <summary>
        /// Sets up the cluster array to be shifted for weighted points.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <param name="points">The full list of points to apply MeanShift with.</param>
        /// <param name="initialClusters">The amount of points to use as clusters.</param>
        /// <returns>An array of <typeparamref name="T"/>s to be clustered.</returns>
        public static (T, int)[] SetupClusters<T>(
            ReadOnlySpan<(T, int)> points,
            int initialClusters)
            where T : unmanaged, IEquatable<T>
        {
            return SetupClusters<(T, int)>(points, initialClusters);
        }

        /// <summary>
        /// Merges really similar clusters, then sorts them by size.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="clusters">The clusters to merge and sort.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <returns>A merged sorted list of clusters.</returns>
        public static (T, int)[] PostProcess<T, TShape, TKernel>(
            T[] clusters,
            TKernel kernel)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            // Remove explict duplicate values.
            DictionarySlim<T, int> mergedCentroidsMap = new DictionarySlim<T, int>();
            foreach (var cluster in clusters)
            {
                mergedCentroidsMap.GetOrAddValueRef(cluster)++;
            }

            // Connected componenents merge.
            // Because convergence may be imperfect, a minimum difference can be used to merge similar clusters.

            // Convert Dictionary to tuple list.
            (T, int)[] mergedCentroids = new (T, int)[mergedCentroidsMap.Count];
            int i = 0;
            foreach (var value in mergedCentroidsMap)
            {
                mergedCentroids[i] = (value.Key, value.Value);
                i++;
            }

            // Apply Connect Components
            mergedCentroids = ConnctedComponents.ConnectComponents<T, TShape, TKernel>(mergedCentroids, kernel);

            Array.Sort(
                mergedCentroids,
                delegate(
                    (T, int) clus1,
                    (T, int) clus2)
                {
                    return clus2.Item2.CompareTo(clus1.Item2);
                });

            return mergedCentroids;
        }

        /// <summary>
        /// Merges really similar clusters, then sorts them by size for weighted clusters.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="clusters">The weighted clusters to merge and sort.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <returns>A merged sorted list of clusters.</returns>
        public static (T, int)[] PostProcess<T, TShape, TKernel>(
            (T, int)[] clusters,
            TKernel kernel)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            // Remove explict duplicate values.
            DictionarySlim<T, int> mergedCentroidsMap = new DictionarySlim<T, int>();
            foreach (var cluster in clusters)
            {
                mergedCentroidsMap.GetOrAddValueRef(cluster.Item1) += cluster.Item2;
            }

            // Connected componenents merge.
            // Because convergence may be imperfect, a minimum difference can be used to merge similar clusters.

            // Convert Dictionary to tuple list.
            (T, int)[] mergedCentroids = new (T, int)[mergedCentroidsMap.Count];
            int i = 0;
            foreach (var value in mergedCentroidsMap)
            {
                mergedCentroids[i] = (value.Key, value.Value);
                i++;
            }

            // Apply Connect Components
            mergedCentroids = ConnctedComponents.ConnectComponents<T, TShape, TKernel>(mergedCentroids, kernel);

            Array.Sort(
                mergedCentroids,
                delegate (
                    (T, int) clus1,
                    (T, int) clus2)
                {
                    return clus2.Item2.CompareTo(clus1.Item2);
                });

            return mergedCentroids;
        }
    }
}
