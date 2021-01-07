using ClusterLib.Abstract;
using ClusterLib.Shapes;
using System.Collections.Generic;

namespace ClusterLib
{
    public class MeanShiftCluster<T, TShape> : Cluster<T, TShape>
        where T : unmanaged
        where TShape : struct, IPoint<T>
    {
        private List<(T, double)> _weightedSubPointList;

        public MeanShiftCluster()
            : base()
        {
            _centroid = null;
            _weightedSubPointList = new List<(T, double)>();
        }

        public MeanShiftCluster(T point)
        {
            _centroid = point;
            _weightedSubPointList = new List<(T, double)>();
            _weightedSubPointList.Add((point, 1));
        }

        internal void Add(T p, double weight = 1)
        {
            _weightedSubPointList.Add((p, weight));
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

        protected override T CalculateCentroid()
        {
            TShape shape = default;
            return shape.WeightedAverage(_weightedSubPointList);
        }
    }
}
