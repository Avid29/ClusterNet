﻿// Adam Dernis © 2021

using ClusterNet.Abstract;

namespace ClusterNet.Shapes
{
    /// <summary>
    /// A Shape for the Point in a <see cref="Cluster{T, TShape}"/>.
    /// </summary>
    /// <typeparam name="T">The type being wrapped by the implementation.</typeparam>
    /// <remarks>
    /// It is highly recommended to use floats for your data.
    /// </remarks>
    public interface IPoint<T>
    {
        /// <summary>
        /// Gets the average value of a list of <typeparamref name="T"/> items.
        /// </summary>
        /// <param name="items">The list of points to average.</param>
        /// <returns>The average of all items in the enumerable.</returns>
        T Average(T[] items);

        /// <summary>
        /// Gets the distance between <paramref name="it1"/> and <paramref name="it2"/>.
        /// </summary>
        /// <param name="it1">Point A.</param>
        /// <param name="it2">Point B.</param>
        /// <returns>The distance between point A and point B.</returns>
        double FindDistanceSquared(T it1, T it2);

        /// <summary>
        /// Gets the weighted average value of a list of (T, double) by point and weight.
        /// </summary>
        /// <param name="items">A weighted list of points.</param>
        /// <returns>The weighted center of the points.</returns>
        T WeightedAverage((T, double)[] items);

        /// <summary>
        /// Checks equality of two items.
        /// </summary>
        /// <param name="it1">The item to compare.</param>
        /// <param name="it2">The item to compare it to.</param>
        /// <param name="error">The accepted error by distance for a comparison.</param>
        /// <returns>Whether or not the items are equal.</returns>
        bool AreEqual(T it1, T it2, double error = 0);
    }
}
