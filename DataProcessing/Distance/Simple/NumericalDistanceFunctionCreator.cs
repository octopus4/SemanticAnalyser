namespace DataPreprocessing.Distance.Simple
{
    public class NumericalDistanceFunctionCreator : IDistanceFunctionCreator
    {
        public DistanceFunction Create(Metric metric)
        {
            return new NumericalDistanceFunction(metric);
        }
    }
}
