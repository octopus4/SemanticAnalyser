namespace DataProcessing.Data.Simple
{
    public class SimpleDataTokenFactory : ITokenFactory
    {
        public DataToken ProduceToken(string[] values, DataType[] typeMask, DataFlow[] flowMask)
        {
            return new SimpleDataToken(values, typeMask, flowMask);
        }
    }
}
