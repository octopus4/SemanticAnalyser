using DataProcessing.Distance;

namespace SOM.Topologies
{
    internal abstract class Topology
    {
        protected double Norm { get; set; }

        public Topology(Metric metric)
        {
            Norm = (double)metric + 1;
        }

        public abstract double Distance(Point p, Point q);
    }
}
