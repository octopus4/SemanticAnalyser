namespace DataPreprocessing.Data.Simple
{
    public class SimpleTokenCreator : ITokenCreator
    {
        public DataToken Create()
        {
            return new SimpleDataToken();
        }
    }
}
