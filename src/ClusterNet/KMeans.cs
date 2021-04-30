using ClusterNet.KMeans;
using ClusterNet.Shapes;
using System;
using System.Runtime.CompilerServices;

namespace ClusterNet
{
    public static partial class ClusterAlgorithms
    {
        /// <inheritdoc cref="Run.KMeans{T, TShape}(ReadOnlySpan{T}, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T, int)[] KMeans<T, TShape>(
            ReadOnlySpan<T> points,
            int clusterCount)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            return Run.KMeans<T, TShape>(points, clusterCount);
        }
    }
}
