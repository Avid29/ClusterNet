// Adam Dernis © 2021

using ClusterNet.KMeans;
using ClusterNet.Shapes;
using System;
using System.Runtime.CompilerServices;

namespace ClusterNet
{
    /// <inheritdoc cref="ClusterAlgorithms"/>
    public static partial class ClusterAlgorithms
    {
        /// <inheritdoc cref="Run.KMeans{T, TShape}(ReadOnlySpan{T}, int)"/>
        public static (T, int)[] KMeans<T, TShape, TAvgProgress>(
            ReadOnlySpan<T> points,
            int clusterCount)
            where T : unmanaged
            where TShape : struct, IPoint<T, TAvgProgress>
        {
            return Run.KMeans<T, TShape, TAvgProgress>(points, clusterCount);
        }
    }
}
