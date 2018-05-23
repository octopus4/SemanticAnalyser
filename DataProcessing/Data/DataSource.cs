using System;
using System.Collections;
using System.Collections.Generic;

namespace DataPreprocessing.Data
{
    /// <summary>
    /// Data source, that provides access to the collection of data tokens
    /// </summary>
    public abstract class DataSource : IEnumerable<DataToken>
    {
        /// <summary>
        /// Static random generator, that is used in the <see cref="GetRandomToken"/> method
        /// </summary>
        private static readonly Random Rng = new Random();

        /// <summary>
        /// Array of data tokens
        /// </summary>
        protected DataToken[] Tokens { get; set; }
        /// <summary>
        /// Mask of data token types
        /// </summary>
        public DataType[] DataTypes {get; private set; }
        /// <summary>
        /// Mask of data token flows
        /// </summary>
        public DataFlow[] FlowTypes { get; private set; }

        /// <summary>
        /// Number of tokens in the data source
        /// </summary>
        public int Length { get { return Tokens.Length; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource"/>
        /// </summary>
        /// <param name="source">Primary data source that has been preprocessed</param>
        /// <param name="typeMask">Mask of the <see cref="DataToken"/> components types</param>
        /// <param name="flowMask">Mask of the <see cref="DataToken"/> components flows</param>
        /// <param name="factory">Token factory</param>
        /// <param name="separator">Separator symbol</param>
        protected DataSource
            (ICollection<string[]> source, DataType[] typeMask, DataFlow[] flowMask, ITokenCreator factory, char? separator = null)
        {
            FlowTypes = flowMask;
            DataTypes = typeMask;

            Tokens = new DataToken[source.Count];
            InitTokens(source, typeMask, flowMask, factory);
        }

        /// <summary>
        /// Initializes tokens in the <see cref="DataSource"/>
        /// </summary>
        /// <param name="data">Data that has been parsed into a collection of string arrays</param>
        /// <param name="typeMask">Mask of the <see cref="DataToken"/> components types</param>
        /// <param name="flowMask">Mask of the <see cref="DataToken"/> components flows</param>
        /// <param name="tokenCreator">Token factory</param>
        private void InitTokens(IEnumerable<string[]> data, DataType[] typeMask, DataFlow[] flowMask, ITokenCreator tokenCreator)
        {
            int index = 0;
            foreach (string[] values in data)
            {
                Tokens[index] = tokenCreator.Create();
                Tokens[index].InitValues(values, typeMask, flowMask);
                index++;
            }
        }

        /// <summary>
        /// Returns randomly selected <see cref="DataToken"/> from the <see cref="DataSource"/>
        /// </summary>
        /// <returns></returns>
        public DataToken GetRandomToken()
        {
            return Tokens[Rng.Next(Length)];
        }

        /// <summary>
        /// Returns an instance of <see cref="IEnumerator{DataToken}"/> that enumerates data tokens in the <see cref="DataSource"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<DataToken> GetEnumerator()
        {
            foreach (DataToken token in Tokens)
            {
                yield return token;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
