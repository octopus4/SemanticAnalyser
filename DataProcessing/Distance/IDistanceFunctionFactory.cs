namespace DataProcessing.Distance
{
    public interface IDistanceFunctionFactory
    {
        DistanceFunction Produce(Metric metric);
    }
}
