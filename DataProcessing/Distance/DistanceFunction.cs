namespace DataProcessing.Distance
{
    public abstract class DistanceFunction
    {
        protected double Norm { get; }

        public DistanceFunction(Metric metric)
        {
            Norm = (int)metric + 1;
        }

        public DistanceFunction(double norm)
        {
            Norm = norm;
        }

        public abstract double Calculate(object[] vector1, object[] vector2);
    }
}
