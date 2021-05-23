// Adam Dernis © 2021

using ClusterNet.GPU.MeanShift;
using ClusterNet.Kernels;
using ColorExtractor.ColorSpaces;
using System;
using System.Numerics;

namespace ClusterNet.GPU
{
    /// <summary>
    /// A class containing all the exposed clustering algorithms.
    /// </summary>
    public static partial class ClusterAlgorithms
    {
        /// <inheritdoc cref="Run.MeanShiftGPU(ReadOnlySpan{Vector3}, GaussianKernel)"/>
        public static (RGBColor, int)[] MeanShiftGPU(
            ReadOnlySpan<RGBColor> points,
            GaussianKernel kernel)
        {
            return Run.MeanShiftGPU(points, kernel);
        }
    }
}
