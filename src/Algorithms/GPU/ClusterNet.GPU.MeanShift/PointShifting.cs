// Adam Dernis © 2021

using ClusterNet.Kernels;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using ComputeSharp;
using System.Numerics;

namespace ClusterNet.GPU.MeanShift
{
    /// <summary>
    /// A class containing method for point shifting on the GPU.
    /// </summary>
    internal class PointShifting
    {
        /// <summary>
        /// Runs shift on a single cluster with the full readonly points.
        /// </summary>
        /// <param name="cluster">The cluster to MeanShift.</param>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="weights">The buffer to save point weights to.</param>
        /// <returns>The fully converged position for <paramref name="cluster"/>.</returns>
        public static RGBColor MeanShiftPoint(
            RGBColor cluster,
            ReadOnlyBuffer<RGBColor> points,
            GaussianKernel kernel,
            ReadWriteBuffer<double> weights)
        {
            RGBShape shape = default;

            // Shift the cluster until it does not shift.
            bool changed = true;
            RGBColor newCluster = default;
            while (changed)
            {
                newCluster = Shift(cluster, points, kernel, weights);
                changed = !shape.AreEqual(newCluster, cluster);
                cluster = newCluster;
            }

            return cluster;
        }

        /// <summary>
        /// Shifts a cluster towards its convergence point. If the cluster is at its convergence point it doesn't move.
        /// </summary>
        /// <param name="p">The cluster to shift.</param>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="weights">The buffer to save point weights to.</param>
        /// <returns>The new cluster from the cluster being shifted.</returns>
        public static RGBColor Shift(
            RGBColor p,
            ReadOnlyBuffer<RGBColor> points,
            GaussianKernel kernel,
            ReadWriteBuffer<double> weights)
        {
            // RGBShape shape = default;
            Gpu.Default.For(points.Length, new PointShiftShader(p, points, weights, kernel));

            // TODO: Convert results to tuple array
            // Better TODO: parallel sum and average.

            return p; // Infinite loop warning! This is just a temporary measure to satisfy the compiler.
        }
    }
}
