namespace DataPreprocessing.Distance.Semantic
{
    public class SemanticDistanceFunctionCreator : IDistanceFunctionCreator
    {
        public DistanceFunction Create(Metric metric)
        {
            return new SemanticDistanceFunction(metric);
        }
    }
}
