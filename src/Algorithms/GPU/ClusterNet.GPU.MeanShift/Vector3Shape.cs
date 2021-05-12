// Adam Dernis © 2021

using ClusterNet.Shapes;
using System.Numerics;

namespace ClusterNet.GPU.MeanShift
{
    /// <summary>
    /// A shape for the <see cref="Vector3"/> struct.
    /// </summary>
    /// <remarks>
    /// This is for while ComputerSharp does not support generics.
    /// </remarks>
    internal struct Vector3Shape : IPoint<Vector3>
    {
        /// <inheritdoc/>
        public bool AreEqual(Vector3 it1, Vector3 it2)
        {
            return it1 == it2;
        }

        /// <inheritdoc/>
        public Vector3 Average(Vector3[] items)
        {
            double sumX, sumY, sumZ;
            sumX = sumY = sumZ = 0;
            int count = 0;

            for (; count < items.Length; count++)
            {
                sumX += items[count].X;
                sumY += items[count].Y;
                sumZ += items[count].Z;
            }

            Vector3 avg = new Vector3()
            {
                X = (float)(sumX / count),
                Y = (float)(sumY / count),
                Z = (float)(sumZ / count),
            };
            return avg;
        }

        /// <inheritdoc/>
        public double FindDistanceSquared(Vector3 it1, Vector3 it2)
        {
            float x = it1.X - it2.X;
            float y = it1.Y - it2.Y;
            float z = it1.Z - it2.Z;

            return (x * x) + (y * y) + (z * z);
        }

        /// <inheritdoc/>
        public Vector3 WeightedAverage((Vector3, double)[] items)
        {
            double sumX, sumY, sumZ;
            sumX = sumY = sumZ = 0;
            double totalWeight = 0;
            for (int i = 0; i < items.Length; i++)
            {
                sumX += items[i].Item1.X * items[i].Item2;
                sumY += items[i].Item1.Y * items[i].Item2;
                sumZ += items[i].Item1.Z * items[i].Item2;
                totalWeight += items[i].Item2;
            }

            Vector3 color = new Vector3()
            {
                X = (float)(sumX / totalWeight),
                Y = (float)(sumY / totalWeight),
                Z = (float)(sumZ / totalWeight),
            };
            return color;
        }
    }
}
