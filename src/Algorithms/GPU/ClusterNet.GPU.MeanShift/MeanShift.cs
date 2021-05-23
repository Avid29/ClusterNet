// Adam Dernis © 2021

using ClusterNet.Kernels;
using ClusterNet.MeanShift;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using ComputeSharp;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (RGBColor, int)[] MeanShiftGPU(
            ReadOnlySpan<RGBColor> points,
            GaussianKernel kernel)
        {
            RGBColor[] clusters = PrePostProcess.SetupClusters(points, 0);

            using ReadOnlyBuffer<RGBColor> pointBuffer = Gpu.Default.AllocateReadOnlyBuffer(points);
            using ReadWriteBuffer<double> weightBuffer = Gpu.Default.AllocateReadWriteBuffer<double>(points.Length);

            for (int i = 0; i < clusters.Length; i++)
            {
                RGBColor cluster = clusters[i];
                clusters[i] = PointShifting.MeanShiftPoint(cluster, pointBuffer, kernel, weightBuffer);
            }

            return PrePostProcess.PostProcess<RGBColor, RGBShape, GaussianKernel>(clusters, kernel);
        }
    }
}
