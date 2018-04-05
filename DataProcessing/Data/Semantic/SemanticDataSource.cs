using System;
using System.Collections.Generic;
using System.Linq;

namespace DataProcessing.Data.Semantic
{
    /// <summary>
    /// Source, which produces, fills and contains collection of <see cref="SemanticDataToken"/>
    /// </summary>
    public class SemanticDataSource : DataSource
    {
        /// <summary>
        /// Default data type of <see cref="DataToken"/>
        /// </summary>
        private static readonly DataType[] DefaultType = new DataType[] { DataType.Semantic, DataType.Semantic, DataType.Semantic };
        /// <summary>
        /// Default data flow of <see cref="DataToken"/>
        /// </summary>
        private static readonly DataFlow[] DefaultFlow = new DataFlow[] { DataFlow.NonUsed, DataFlow.Input, DataFlow.Input };
        /// <summary>
        /// Word separators, which are needed to split the text to the array of separte words
        /// </summary>
        private static readonly char[] WordSeparators = new char[] { ' ', '.', ',', '?', '!', '"', '-', ';' };
        /// <summary>
        /// Semantic matrix that contains left and right context of an each word in the text
        /// </summary>
        public static SemanticMatrix Matrix { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="SemanticDataSource"/>
        /// </summary>
        /// <param name="text">Text on any language (natural or artificial)</param>
        /// <param name="contextSize">Size of the context in each <see cref="SemanticDataToken"/></param>
        public SemanticDataSource(string text, int contextSize, int contextDistance)
            : base(Parse(text, contextSize, contextDistance), DefaultType, DefaultFlow, new SemanticDataTokenFactory())
        {
        }

        /// <summary>
        /// Parses text to the array of pre-tokens
        /// </summary>
        /// <param name="text">Input text</param>
        /// <param name="contextSize">Size of the context in each <see cref="SemanticDataToken"/></param>
        /// <returns></returns>
        private static ICollection<string[]> Parse(string text, int contextSize, int contextDistance)
        {
            string[] words = text.ToLower().Split(WordSeparators, StringSplitOptions.RemoveEmptyEntries);
            string[] distinctWords = words.Distinct().ToList().ToArray();
            SemanticDataToken.ContextSize = (int)(distinctWords.Length * (contextSize / 100.0));

            Matrix = new SemanticMatrix(words, distinctWords.ToArray(), contextDistance);

            return Matrix;
        }

        public override string ToString()
        {
            return Matrix.ToString();
        }
    }
}
