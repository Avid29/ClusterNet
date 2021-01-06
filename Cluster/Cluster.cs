using ClusterLib.Shapes;
using System;
using System.Collections.Generic;

namespace ClusterLib
{
    public class Cluster<T, TShape>
        where T : unmanaged
        where TShape : struct, IPoint<T>
    {
        private T? _centroid;
        private List<(T, double)> _weightedSubPointList;
        private double _weightSum = 0;

        /// <summary>
        /// The center of the point.
        /// </summary>
        public T Centroid
        {
            get => _centroid ?? (T)(_centroid = CalculateCentroid());
            set => _centroid = value;
        }

        public Cluster()
            : base()
        {
            _centroid = null;
            _weightedSubPointList = new List<(T, double)>();
        }

        public Cluster(T point)
        {
            _centroid = point;
            _weightedSubPointList = new List<(T, double)>();
            _weightedSubPointList.Add((point, 1));
        }

        internal void Add(T p, double weight)
        {
            _weightedSubPointList.Add((p, weight));
            _weightSum += weight;
            _centroid = null;
        }

        internal T GetNearestToCenter()
        {
            TShape shape = default;

            double minimumDistance = 0.0;
            int nearestPointIndex = -1;

            for (int i = 0; i < _weightedSubPointList.Count; i++)
            {
                T p = _weightedSubPointList[i].Item1;
                double distance = shape.FindDistance(p, Centroid);

                if (i == 0)
                {
                    minimumDistance = distance;
                    nearestPointIndex = i;
                }
                else
                {
                    if (minimumDistance > distance)
                    {
                        minimumDistance = distance;
                        nearestPointIndex = i;
                    }
                }
            }

            return _weightedSubPointList[nearestPointIndex].Item1;
        }

        private T CalculateCentroid()
        {
            T sum = default;
            TShape shape = default;

            foreach ((T, double) item in _weightedSubPointList)
                sum = shape.Sum(sum, item.Item1, item.Item2);

            sum = shape.Divide(sum, _weightSum);
            return sum;
        }
    }
}
