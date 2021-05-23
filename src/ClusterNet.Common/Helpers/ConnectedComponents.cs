// Adam Dernis © 2021

using ClusterNet.Kernels;
using ClusterNet.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusterNet.Helpers
{
    /// <summary>
    /// A class containing methods to run connect components.
    /// </summary>
    public static class ConnectedComponents
    {
        /// <summary>
        /// Merges similar clusters.
        /// </summary>
        /// <typeparam name="T">The type of cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the clusters to merge.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="clusters">The clusters to merge under a minimum difference.</param>
        /// <param name="kernel">The kernel that contains the min difference to merge by.</param>
        /// <returns>A smaller list of clusters that have been merged by similar distances.</returns>
        public static (T, int)[] ConnectComponents<T, TShape, TKernel>(
            (T, int)[] clusters,
            TKernel kernel)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            TShape shape = default;

            List<List<(T, int)>> connectedComponents = new List<List<(T, int)>>(clusters.Length);
            foreach (var cluster in clusters)
            {
                // Determine which existing connections this cluster connects to
                // and track their indicies.
                List<int> connections = new List<int>(connectedComponents.Count);
                for (int i = 0; i < connectedComponents.Count; i++)
                {
                    foreach (var component in connectedComponents[i])
                    {
                        if (shape.FindDistanceSquared(component.Item1, cluster.Item1) < kernel.WindowSize)
                        {
                            connections.Add(i);
                            break;
                        }
                    }
                }

                if (connections.Count == 0)
                {
                    // Add new connection
                    connectedComponents.Add(new List<(T, int)>());
                    connectedComponents.Last().Add(cluster);
                }
                else if (connections.Count == 1)
                {
                    // Add to overlapping connection
                    connectedComponents[connections[0]].Add(cluster);
                }
                else
                {
                    // Merge overlapping connections
                    // Remove start from back so indicies aren't changed.
                    for (int i = connections.Count - 1; i > 0; i--)
                    {
                        connectedComponents[connections[0]].AddRange(connectedComponents[connections[i]]);
                        connectedComponents.RemoveAt(connections[i]);
                    }
                }
            }

            (T, int)[] mergedClusters = new (T, int)[connectedComponents.Count];
            for (int i = 0; i < mergedClusters.Length; i++)
            {
                List<(T, int)> connection = connectedComponents[i];
                (T, double)[] points = new (T, double)[connection.Count];
                int count = 0;
                for (int j = 0; j < connection.Count; j++)
                {
                    points[j] = connection[j];
                    count += connection[j].Item2;
                }

                mergedClusters[i] = (shape.WeightedAverage(points), count);
            }

            return mergedClusters;
        }
    }
}
