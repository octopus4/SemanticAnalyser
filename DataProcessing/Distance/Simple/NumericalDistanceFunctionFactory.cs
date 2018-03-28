namespace DataProcessing.Distance.Simple
{
    public class NumericalDistanceFunctionFactory : IDistanceFunctionFactory
    {
        public DistanceFunction Produce(Metric metric)
        {
            return new NumericalDistanceFunction(metric);
        }
    }
}
