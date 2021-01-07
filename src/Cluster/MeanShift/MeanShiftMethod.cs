using ClusterLib.Kernels;
using ClusterLib.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace ClusterLib
{
    /// <summary>
    /// A class containing methods related to the MeanShift algorithm.
    /// </summary>
    public static class MeanShiftMethod
    {
        /// <summary>
        /// Runs MeanShift cluster on a list of <typeparamref name="T"/> points.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static List<(MeanShiftCluster<T, TShape>, int)> MeanShift<T, TShape>(
            IEnumerable<T> points,
            IKernel kernel)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;

            // Create a cluster for each point.
            List<MeanShiftCluster<T, TShape>> clusters = points.Select(x => new MeanShiftCluster<T, TShape>(x)).ToList();
            
            // Shift each cluster until it's at its convergence point.
            for (int i = 0; i < clusters.Count; i++)
            {
                MeanShiftCluster<T, TShape> cluster = clusters[i];
                MeanShiftCluster<T, TShape> newCluster = default;
                bool changed = true;

                // Shift the cluster until it does not shift.
                while (changed)
                {
                    newCluster = Shift(cluster, points, kernel);
                    changed = shape.FindDistanceSquared(newCluster.Centroid, cluster.Centroid) != 0;
                    cluster = newCluster;
                }

                // Replace original cluster with shifted cluster.
                clusters[i] = newCluster;
            }

            // Remove duplicate clusters.
            Dictionary<T, int> pointDictionary = new Dictionary<T, int>();
            for (int i = 0; i < clusters.Count; i++)
            {
                var cluster = clusters[i];
                if (!pointDictionary.ContainsKey(cluster.Centroid))
                {
                    pointDictionary.Add(cluster.Centroid, 1);
                } else
                {
                    pointDictionary[cluster.Centroid]++;
                }
            }

            return clusters.Select(x => (x, pointDictionary[x.Centroid])).ToList();
        }

        /// <summary>
        /// Shifts a cluster towards its convergence point.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="p">The cluster to shift.</param>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <returns>The new cluster from the cluster being shifted.</returns>
        private static MeanShiftCluster<T, TShape> Shift<T, TShape>(
            MeanShiftCluster<T, TShape> p,
            IEnumerable<T> points,
            IKernel kernel)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;


            // Create cluster based on distance of points from the current cluster's centroid
            MeanShiftCluster<T, TShape> newCluster = new MeanShiftCluster<T, TShape>();
            foreach (T point in points)
            {
                double distance = shape.FindDistanceSquared(p.Centroid, point);
                double weight = kernel.WeightDistance(distance);
                newCluster.Add(point, weight);
            }

            return newCluster;
        }
    }
}
