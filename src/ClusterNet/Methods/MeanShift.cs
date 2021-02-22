using ClusterNet.Kernels;
using ClusterNet.MeanShift;
using ClusterNet.Shapes;
using System;
using System.Runtime.CompilerServices;

namespace ClusterNet.Methods
{
    /// <summary>
    /// A class containing all the exposed clustering algorithms.
    /// </summary>
    public static partial class ClusterAlgorithms
    {
        /// <inheritdoc cref="Run.MeanShift{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe (T, int)[] MeanShift<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            return Run.MeanShift<T, TShape, TKernel>(points, kernel, initialClusters);
        }

        /// <inheritdoc cref="Run.MeanShiftMultiThreaded{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe (T, int)[] MeanShiftMultiThreaded<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            return Run.MeanShiftMultiThreaded<T, TShape, TKernel>(points, kernel, initialClusters);
        }

        /// <inheritdoc cref="RunWeighted.WeightedMeanShift{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe (T, int)[] WeightedMeanShift<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            return RunWeighted.WeightedMeanShift<T, TShape, TKernel>(points, kernel, initialClusters);
        }

        /// <inheritdoc cref="RunWeighted.WeightedMeanShiftMultiThreaded{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe (T, int)[] WeightedMeanShiftMultiThreaded<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            return RunWeighted.WeightedMeanShiftMultiThreaded<T, TShape, TKernel>(points, kernel, initialClusters);
        }

        /// <inheritdoc cref="RunWeighted.WeightedMeanShiftMultiThreaded{T, TShape, TKernel}(ReadOnlySpan{(T, int)}, TKernel, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe (T, int)[] WeightedMeanShift<T, TShape, TKernel>(
            ReadOnlySpan<(T, int)> weightedPoints,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            return RunWeighted.WeightedMeanShift<T, TShape, TKernel>(weightedPoints, kernel, initialClusters);
        }

        /// <inheritdoc cref="RunWeighted.WeightedMeanShiftMultiThreaded{T, TShape, TKernel}(ReadOnlySpan{(T, int)}, TKernel, int)/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe (T, int)[] WeightedMeanShiftMultiThreaded<T, TShape, TKernel>(
            ReadOnlySpan<(T, int)> weightedPoints,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            return RunWeighted.WeightedMeanShiftMultiThreaded<T, TShape, TKernel>(weightedPoints, kernel, initialClusters);
        }
    }
}