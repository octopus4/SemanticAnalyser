﻿namespace DataPreprocessing.Data.Semantic
{
    public class SemanticTokenCreator : ITokenCreator
    {
        public DataToken Create()
        {
            return new SemanticDataToken();
        }
    }
}