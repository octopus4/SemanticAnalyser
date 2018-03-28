namespace DataProcessing.Data
{
    public interface ITokenFactory
    {
        DataToken ProduceToken(string[] values, DataType[] typeMask, DataFlow[] flowMask);
    }
}
