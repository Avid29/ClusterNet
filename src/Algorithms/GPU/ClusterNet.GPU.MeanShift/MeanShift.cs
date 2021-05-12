// Adam Dernis © 2021

using ClusterNet.Kernels;
using ClusterNet.MeanShift;
using ComputeSharp;
using System;
using System.Numerics;

namespace ClusterNet.GPU.MeanShift
{
    /// <summary>
    /// A class containing root implementations of MeanShift algorithms for the GPU.
    /// </summary>
    internal static class Run
    {
        /// <summary>
        /// Runs MeanShift cluster on a list of Vector3 points with GPU acceleration.
        /// </summary>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static (Vector3, int)[] MeanShiftGPU(
            ReadOnlySpan<Vector3> points,
            GaussianKernel kernel)
        {
            Vector3[] clusters = PrePostProcess.SetupClusters(points, 0);

            using ReadOnlyBuffer<Vector3> pointBuffer = Gpu.Default.AllocateReadOnlyBuffer(points);
            using ReadWriteBuffer<double> weightBuffer = Gpu.Default.AllocateReadWriteBuffer<double>(points.Length);

            for (int i = 0; i < clusters.Length; i++)
            {
                Vector3 cluster = clusters[i];
                clusters[i] = PointShifting.MeanShiftPoint(cluster, pointBuffer, kernel, weightBuffer);
            }

            return PrePostProcess.PostProcess<Vector3, Vector3Shape, GaussianKernel>(clusters, kernel);
        }
    }
}
