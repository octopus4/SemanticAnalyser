using DataProcessing.Distance;
using DataProcessing.Distance.Simple;

namespace SOM.Topologies
{
    internal class RectangleTopology : Topology
    {
        private DistanceFunction DistanceFunction { get; }

        public RectangleTopology(Metric metric) : base(metric)
        {
            DistanceFunction = new NumericalDistanceFunction(metric);
        }

        public override double Distance(Point p, Point q)
        {
            return DistanceFunction.Calculate
                (new object[] { p.X, p.Y },
                new object[] { q.X, q.Y });
        }
    }
}
