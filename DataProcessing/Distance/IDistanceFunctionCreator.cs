namespace DataProcessing.Distance
{
    public interface IDistanceFunctionCreator
    {
        DistanceFunction Create(Metric metric);
    }
}
