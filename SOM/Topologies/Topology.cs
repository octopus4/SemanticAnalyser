using DataPreprocessing.Distance;

namespace SOM.Topologies
{
    internal abstract class Topology
    {
        protected double Norm { get; set; }

        protected Topology(Metric metric)
        {
            Norm = (double)metric + 1;
        }

        public abstract double Distance(Point p, Point q);
    }
}
