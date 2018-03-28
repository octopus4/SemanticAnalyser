namespace DataProcessing.Data.Semantic
{
    public class SemanticDataTokenFactory : ITokenFactory
    {
        public DataToken ProduceToken(string[] values, DataType[] typeMask, DataFlow[] flowMask)
        {
            return new SemanticDataToken(values, typeMask, flowMask);
        }
    }
}