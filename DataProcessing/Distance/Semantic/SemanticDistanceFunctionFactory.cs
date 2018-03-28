namespace DataProcessing.Distance.Semantic
{
    public class SemanticDistanceFunctionFactory : IDistanceFunctionFactory
    {
        public DistanceFunction Produce(Metric metric)
        {
            return new SemanticDistanceFunction(metric);
        }
    }
}
