// Adam Dernis © 2021

using ClusterNet.Kernels;
using ClusterNet.MeanShift;
using ClusterNet.Shapes;
using System;
using System.Runtime.CompilerServices;

namespace ClusterNet
{
    /// <summary>
    /// A class containing all the exposed clustering algorithms.
    /// </summary>
    public static partial class ClusterAlgorithms
    {
        /// <inheritdoc cref="Run.MeanShift{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/>
        public static unsafe (T, int)[] MeanShift<T, TShape, TKernel, TAvgProgress>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T, TAvgProgress>
            where TKernel : struct, IKernel
        {
            return Run.MeanShift<T, TShape, TKernel, TAvgProgress>(points, kernel, initialClusters);
        }

        /// <inheritdoc cref="Run.MeanShiftMultiThreaded{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/>
        public static unsafe (T, int)[] MeanShiftMultiThreaded<T, TShape, TKernel, TAvgProgress>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T, TAvgProgress>
            where TKernel : struct, IKernel
        {
            return Run.MeanShiftMultiThreaded<T, TShape, TKernel, TAvgProgress>(points, kernel, initialClusters);
        }

        /// <inheritdoc cref="Run.MeanShiftFixedThreaded{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int, int)"/>
        public static unsafe (T, int)[] MeanShiftFixedThreaded<T, TShape, TKernel, TAvgProgress>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0,
            int threadCount = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T, TAvgProgress>
            where TKernel : struct, IKernel
        {
            return Run.MeanShiftFixedThreaded<T, TShape, TKernel, TAvgProgress>(points, kernel, initialClusters, threadCount);
        }
    }
}
