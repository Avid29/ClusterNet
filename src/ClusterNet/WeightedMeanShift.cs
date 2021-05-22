// Adam Dernis © 2021

using ClusterNet.Kernels;
using ClusterNet.MeanShift;
using ClusterNet.Shapes;
using System;
using System.Runtime.CompilerServices;

namespace ClusterNet
{
    /// <inheritdoc cref="ClusterAlgorithms"/>
    public static partial class ClusterAlgorithms
    {
        /// <inheritdoc cref="RunWeighted.WeightedMeanShift{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/>
        public static unsafe (T, int)[] WeightedMeanShift<T, TShape, TKernel, TAvgProgress>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T, TAvgProgress>
            where TKernel : struct, IKernel
        {
            return RunWeighted.WeightedMeanShift<T, TShape, TKernel, TAvgProgress>(points, kernel, initialClusters);
        }

        /// <inheritdoc cref="RunWeighted.WeightedMeanShiftMultiThreaded{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/>
        public static unsafe (T, int)[] WeightedMeanShiftMultiThreaded<T, TShape, TKernel, TAvgProgress>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T, TAvgProgress>
            where TKernel : struct, IKernel
        {
            return RunWeighted.WeightedMeanShiftMultiThreaded<T, TShape, TKernel, TAvgProgress>(points, kernel, initialClusters);
        }

        /// <inheritdoc cref="RunWeighted.WeightedMeanShiftFixedThreaded{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int, int)"/>
        public static unsafe (T, int)[] WeightedMeanShiftFixedThreaded<T, TShape, TKernel, TAvgProgress>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T, TAvgProgress>
            where TKernel : struct, IKernel
        {
            return RunWeighted.WeightedMeanShiftFixedThreaded<T, TShape, TKernel, TAvgProgress>(points, kernel, initialClusters);
        }

        /// <inheritdoc cref="RunWeighted.WeightedMeanShift{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/>
        public static unsafe (T, int)[] WeightedMeanShift<T, TShape, TKernel, TAvgProgress>(
            ReadOnlySpan<(T, int)> weightedPoints,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T, TAvgProgress>
            where TKernel : struct, IKernel
        {
            return RunWeighted.WeightedMeanShift<T, TShape, TKernel, TAvgProgress>(weightedPoints, kernel, initialClusters);
        }

        /// <inheritdoc cref="RunWeighted.WeightedMeanShiftMultiThreaded{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int)"/>
        public static unsafe (T, int)[] WeightedMeanShiftMultiThreaded<T, TShape, TKernel, TAvgProgress>(
            ReadOnlySpan<(T, int)> weightedPoints,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T, TAvgProgress>
            where TKernel : struct, IKernel
        {
            return RunWeighted.WeightedMeanShiftMultiThreaded<T, TShape, TKernel, TAvgProgress>(weightedPoints, kernel, initialClusters);
        }

        /// <inheritdoc cref="RunWeighted.WeightedMeanShiftFixedThreaded{T, TShape, TKernel}(ReadOnlySpan{T}, TKernel, int, int)"/>
        public static unsafe (T, int)[] WeightedMeanShiftFixedThreaded<T, TShape, TKernel, TAvgProgress>(
            ReadOnlySpan<(T, int)> weightedPoints,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T, TAvgProgress>
            where TKernel : struct, IKernel
        {
            return RunWeighted.WeightedMeanShiftFixedThreaded<T, TShape, TKernel, TAvgProgress>(weightedPoints, kernel, initialClusters);
        }
    }
}
