using System.Collections.Generic;

namespace DataPreprocessing.Data.Simple
{
    /// <summary>
    /// Simple data source, that is used for numeric, categorial and mixed data
    /// </summary>
    public class SimpleDataSource : DataSource
    {
        /// <summary>
        /// Separator symbol, that is used in the <see cref="Parse(string[], char?)"/> method
        /// </summary>
        private static char Separator = '\t';

        public SimpleDataSource(string[] source, DataType[] typeMask, DataFlow[] flowMask, char? separator = null)
            : base(Parse(source, separator), typeMask, flowMask, new SimpleTokenCreator(), separator)
        {
        }

        private static ICollection<string[]> Parse(string[] source, char? separator)
        {
            if (separator != null)
            {
                Separator = separator.Value;
            }

            List<string[]> result = new List<string[]>(source.Length);
            foreach (string line in source)
            {
                result.Add(line.Split(Separator));
            }

            return result;
        }
    }
}
