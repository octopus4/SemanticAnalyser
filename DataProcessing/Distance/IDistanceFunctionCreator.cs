namespace DataPreprocessing.Distance
{
    public interface IDistanceFunctionCreator
    {
        DistanceFunction Create(Metric metric);
    }
}
