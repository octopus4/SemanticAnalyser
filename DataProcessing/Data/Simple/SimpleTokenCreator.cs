namespace DataProcessing.Data.Simple
{
    public class SimpleTokenCreator : ITokenCreator
    {
        public DataToken Create()
        {
            return new SimpleDataToken();
        }
    }
}
